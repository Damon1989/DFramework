using System;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DFramework.Domain;
using DFramework.Infrastructure;
using DFramework.Repositories;

namespace DFramework.EntityFramework
{
    public class MSDbContext : DbContext, IDbContext
    {
        private ObjectContext _objectContext;

        public MSDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            InitObjectContext();
        }

        protected void InitObjectContext()
        {
            _objectContext = (this as IObjectContextAdapter).ObjectContext;
            if (_objectContext != null)
            {
                _objectContext.ObjectMaterialized +=
                    (s, e) => this.InitializeQueryableCollections(e.Entity);
            }
        }

        public void Reload<TEntity>(TEntity entity) where TEntity : class
        {
            var entry = Entry(entity);
            entry.Reload();
            (entry as AggregateRoot)?.Rollback();
        }

        public async Task ReloadAsync<TEntity>(TEntity entity) where TEntity : class
        {
            var entry = Entry(entity);
            await entry.ReloadAsync()
                .ConfigureAwait(false);
            (entity as AggregateRoot)?.Rollback();
        }

        public void RemoveEntity<TEntity>(TEntity entity) where TEntity : class
        {
            var entry = Entry(entity);
            if (entry != null)
            {
                entry.State = EntityState.Deleted;
            }
        }

        public virtual void Rollback()
        {
            ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Deleted)
                .ForEach(e => { e.State = EntityState.Detached; });
            var refreshableObjects = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified || e.State == EntityState.Unchanged)
                .Select(e => e.Entity);
            _objectContext.Refresh(RefreshMode.StoreWins, refreshableObjects);
            ChangeTracker.Entries().ForEach(e => { (e.Entity as AggregateRoot)?.Rollback(); });
        }

        public override int SaveChanges()
        {
            try
            {
                ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Added)
                    .ForEach(e => { this.InitializeQueryableCollections(e.Entity); });
                return base.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Rollback();
                throw new OptimisticConcurrencyException(ex.Message, ex);
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessage = string.Join(";", ex.EntityValidationErrors
                    .SelectMany(eve => eve.ValidationErrors
                        .Select(e => new { eve.Entry, Error = e })
                        .Select(e =>
                            $"{e.Entry?.Entity?.GetType().Name}:{e.Error?.PropertyName} / {e.Error?.ErrorMessage}")));
                throw new Exception(errorMessage, ex);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.GetBaseException().Message, ex);
            }
        }

        public override Task<int> SaveChangesAsync()
        {
            return SaveChangesAsync(CancellationToken.None);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            try
            {
                ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Added)
                    .ForEach(e => { this.InitializeQueryableCollections(e.Entity); });
                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Rollback();
                throw new OptimisticConcurrencyException(ex.Message, ex);
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessage = string.Join(";", ex.EntityValidationErrors
                    .SelectMany(eve => eve.ValidationErrors
                        .Select(e => new { eve.Entry, Error = e })
                        .Select(e =>
                            $"{e.Entry?.Entity?.GetType().Name}:{e.Error?.PropertyName} / {e.Error?.ErrorMessage}")));
                throw new Exception(errorMessage, ex);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.GetBaseException().Message, ex);
            }
        }
    }
}