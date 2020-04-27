namespace DFramework.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DCommon;

    using DFramework.Repositories;

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

        public void ClearCollection<TEntity>(ICollection<TEntity> collection)
            where TEntity : class
        {
            var entities = collection.ToList();
            collection.Clear();
            entities.ForEach(e => { this._domainContext?.RemoveEntity(e); });
        }

        public TContext GetDbContext<TContext>()
            where TContext : class
        {
            return this._domainContext as TContext;
        }

        public void Reload()
        {
            if (this._domainContext == null) throw new NullReferenceException(nameof(this._domainContext));
            this._domainContext.Reload(this);
            (this as AggregateRoot)?.Rollback();
        }

        public async Task ReloadAsync()
        {
            if (this._domainContext == null) throw new NullReferenceException(nameof(this._domainContext));
            await this._domainContext.ReloadAsync(this).ConfigureAwait(false);
            (this as AggregateRoot)?.Rollback();
        }

        public void RemoveCollectionEntities<TEntity>(ICollection<TEntity> collection, params TEntity[] entities)
            where TEntity : class
        {
            entities?.ForEach(
                e =>
                    {
                        collection.Remove(e);
                        this._domainContext?.RemoveEntity(e);
                    });
        }

        internal void SetDomainContext(IDbContext domainContext)
        {
            this._domainContext = domainContext;
        }
    }
}