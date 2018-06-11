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

namespace DFramework.EntityFramework
{
    public class UnitOfWork : IUnitOfWork
    {
        protected List<MSDbContext> _dbContexts;

        //protected IEventBus _eventBus;
        protected Exception _exception;

        protected ILogger _logger;

        public UnitOfWork(
            ILoggerFactory loggerFactory)
        {
            _dbContexts = new List<MSDbContext>();
            _logger = loggerFactory.Create(GetType().Name);
        }

        //public UnitOfWork(IEventBus eventBus,
        //                  ILoggerFactory loggerFactory)
        //{
        //    _dbContexts = new List<MSDbContext>();
        //    _eventBus = eventBus;
        //    _logger = loggerFactory.Create(GetType().Name);
        //}

        public void Dispose()
        {
            _dbContexts.ForEach(_dbCx => _dbCx.Dispose());
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
                    _dbContexts.ForEach(dbContext =>
                    {
                        dbContext.SaveChanges();
                        dbContext.ChangeTracker.Entries()
                            .ForEach(e =>
                            {
                                if (e.Entity is AggregateRoot root)
                                {
                                    //_eventBus?.Publish(root.GetDomainEvents());
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
                    foreach (var dbContext in _dbContexts)
                    {
                        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                        dbContext.ChangeTracker.Entries().ForEach(e =>
                        {
                            if (e.Entity is AggregateRoot)
                            {
                                //_eventBus?.Publish((e.Entity as AggregateRoot).GetDomainEvents());
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
            _dbContexts.ForEach(dbCx => { dbCx.Rollback(); });
        }

        internal void RegisterDbContext(MSDbContext dbContext)
        {
            if (!_dbContexts.Exists(dbCtx => dbCtx.Equals(dbContext)))
            {
                _dbContexts.Add(dbContext);
            }
        }
    }
}