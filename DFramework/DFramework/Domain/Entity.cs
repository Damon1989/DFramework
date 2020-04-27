using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFramework.Infrastructure;
using DFramework.Repositories;
using DCommon;

namespace DFramework.Domain
{
    public static class PocoContextInitializer
    {
        public static void InitializeQueryableCollections(this IDbContext context, object entity)
        {
            (entity as Entity)?.SetDomainContext(context);
        }
    }

    public class Entity : IEntity
    {
        private IDbContext _domainContext;

        internal void SetDomainContext(IDbContext domainContext)
        {
            _domainContext = domainContext;
        }

        public void ClearCollection<TEntity>(ICollection<TEntity> collection)
            where TEntity : class
        {
            var entities = collection.ToList();
            collection.Clear();
            entities.ForEach(e =>
            {
                _domainContext?.RemoveEntity(e);
            });
        }

        public void RemoveCollectionEntities<TEntity>(ICollection<TEntity> collection, params TEntity[] entities)
            where TEntity : class
        {
            entities?.ForEach(e =>
            {
                collection.Remove(e);
                _domainContext?.RemoveEntity(e);
            });
        }

        public void Reload()
        {
            if (_domainContext == null)
            {
                throw new NullReferenceException(nameof(_domainContext));
            }
            _domainContext.Reload(this);
            (this as AggregateRoot)?.Rollback();
        }

        public async Task ReloadAsync()
        {
            if (_domainContext == null)
            {
                throw new NullReferenceException(nameof(_domainContext));
            }
            await _domainContext.ReloadAsync(this)
                                .ConfigureAwait(false);
            (this as AggregateRoot)?.Rollback();
        }

        public TContext GetDbContext<TContext>()
            where TContext : class
        {
            return _domainContext as TContext;
        }
    }
}