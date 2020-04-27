using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using DFramework.Domain;
using DFramework.Event;
using DFramework.Infrastructure;
using DFramework.Infrastructure.Logging;
using DFramework.UnitOfWork;
using DCommon;

namespace DFramework.EntityFramework
{
    public class UnitOfWork : IUnitOfWork
    {
        protected List<MSDbContext> DbContexts;

        protected IEventBus EventBus;
        protected Exception Exception;

        protected ILogger Logger;

        public UnitOfWork(
            ILoggerFactory loggerFactory)
        {
            DbContexts = new List<MSDbContext>();
            Logger = loggerFactory.Create(GetType().Name);
        }

        public UnitOfWork(
                          ILoggerFactory loggerFactory,
            IEventBus eventBus)
        {
            DbContexts = new List<MSDbContext>();
            EventBus = eventBus;
            Logger = loggerFactory.Create(GetType().Name);
        }

        public void Dispose()
        {
            DbContexts.ForEach(_dbCx => _dbCx.Dispose());
        }

        protected virtual void BeforeCommit()
        {
        }

        protected virtual void AfterCommit()
        {
        }

        public virtual void Commit(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            TransactionScopeOption scopeOption = TransactionScopeOption.Required)
        {
            try
            {
                using (var scope = new TransactionScope(scopeOption,
                    new TransactionOptions { IsolationLevel = isolationLevel },
                    TransactionScopeAsyncFlowOption.Enabled))
                {
                    DbContexts.ForEach(dbContext =>
                    {
                        dbContext.SaveChanges();
                        dbContext.ChangeTracker.Entries()
                            .ForEach(e =>
                            {
                                if (e.Entity is AggregateRoot root)
                                {
                                    EventBus?.Publish(root.GetDomainEvents());
                                }
                            });
                    });
                    BeforeCommit();
                    scope.Complete();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                AfterCommit();
            }
        }

        public Task CommitAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            TransactionScopeOption scopeOption = TransactionScopeOption.Required)
        {
            return CommitAsync(CancellationToken.None, isolationLevel, scopeOption);
        }

        public virtual async Task CommitAsync(CancellationToken cancellationToken, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            TransactionScopeOption scopeOption = TransactionScopeOption.Required)
        {
            try
            {
                using (var scope = new TransactionScope(scopeOption,
                    new TransactionOptions { IsolationLevel = isolationLevel },
                    TransactionScopeAsyncFlowOption.Enabled))
                {
                    foreach (var dbContext in DbContexts)
                    {
                        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                        dbContext.ChangeTracker.Entries().ForEach(e =>
                        {
                            if (e.Entity is AggregateRoot)
                            {
                                EventBus?.Publish((e.Entity as AggregateRoot).GetDomainEvents());
                            }
                        });
                    }

                    BeforeCommit();
                    scope.Complete();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                AfterCommit();
            }
        }

        public void Rollback()
        {
            DbContexts.ForEach(dbCx => { dbCx.Rollback(); });
        }

        internal void RegisterDbContext(MSDbContext dbContext)
        {
            if (!DbContexts.Exists(dbCtx => dbCtx.Equals(dbContext)))
            {
                DbContexts.Add(dbContext);
            }
        }
    }
}