using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DFramework.Domain;
using DFramework.Infrastructure;

namespace DFramework.EntityFramework
{
    public static class EntityExtension
    {
        public static void Reload<TEntity>(this TEntity entity)
        where TEntity : Entity
        {
            var dbContext = entity.GetDbContext<MSDbContext>();
            if (dbContext == null)
            {
                throw new NullReferenceException(nameof(dbContext));
            }
            dbContext.Reload(entity);
            (entity as AggregateRoot)?.Rollback();
        }

        public static async Task ReloadAsync<TEntity>(this TEntity entity)
            where TEntity : Entity
        {
            var dbCotnext = entity.GetDbContext<MSDbContext>();
            if (dbCotnext == null)
            {
                throw new NullReferenceException(nameof(dbCotnext));
            }
            await dbCotnext.ReloadAsync(entity)
                .ConfigureAwait(false);
            (entity as AggregateRoot)?.Rollback();
        }

        public static TContext GetDbContext<TContext>(this Entity entity) where TContext : class
        {
            return entity.GetValueByKey<TContext>("DomainContext");
        }

        public static DbEntityEntry<TEntity> GetDbEntityEntry<TEntity>(this TEntity entity)
            where TEntity : Entity
        {
            return entity.GetDbContext<MSDbContext>()?.Entry(entity);
        }

        public static void MarkAsDeleted(this Entity entity)
        {
            var entry = entity.GetDbEntityEntry();
            if (entry != null)
                entry.State = EntityState.Deleted;
        }

        public static void MarkAsAdded(this Entity entity)
        {
            var entry = entity.GetDbEntityEntry();
            if (entry != null)
                entry.State = EntityState.Added;
        }

        public static void MarkAsModified(this Entity entity)
        {
            var entry = entity.GetDbEntityEntry();
            if (entry != null)
                entry.State = EntityState.Modified;
        }

        public static void MarkAsUnchanged(this Entity entity)
        {
            var entry = entity.GetDbEntityEntry();
            if (entry != null)
                entry.State = EntityState.Unchanged;
        }

        public static IQueryable<TElement> GetQueryable<TElement>(this Entity entity,
                                                                  string collectionName)
            where TElement : class
        {
            IQueryable<TElement> query = null;
            var entry = entity.GetDbEntityEntry();
            if (entry != null)
                query = entry.Collection(collectionName).Query().Cast<TElement>();
            return query;
        }

        public static IQueryable<TElement> GetQueryable<TEntity, TElement>(this TEntity entity,
            Expression<Func<TEntity, ICollection<TElement>>> navigationProperty)
            where TEntity : Entity
            where TElement : class
        {
            IQueryable<TElement> query = null;
            var entry = entity.GetDbEntityEntry();
            if (entry != null)
                query = entry.Collection(navigationProperty).Query().Cast<TElement>();
            return query;
        }

        public static void ReferenceLoad<TEntity, TProperty>(this TEntity entity,
            Expression<Func<TEntity, TProperty>> navigationProperty)
            where TEntity : Entity
            where TProperty : class
        {
            var entry = entity.GetDbEntityEntry();
            if (entry != null)
                entity.GetDbEntityEntry().Reference(navigationProperty).Load();
        }

        public static void ReferenceLoad<TProperty>(this Entity entity, string navigationPropertyName)
            where TProperty : class
        {
            var entry = entity.GetDbEntityEntry();
            if (entry != null)
                entity.GetDbEntityEntry().Reference(navigationPropertyName).Load();
        }

        public static void CollectionLoad<TEntity, TElement>(this TEntity entity,
            Expression<Func<TEntity, ICollection<TElement>>> navigationProperty)
            where TEntity : Entity
            where TElement : class
        {
            var entry = entity.GetDbEntityEntry();
            if (entry != null)
                entity.GetDbEntityEntry().Collection(navigationProperty).Load();
        }

        public static void CollectionLoad<TElement>(this Entity entity, string navigationPropertyName)
            where TElement : class
        {
            var entry = entity.GetDbEntityEntry();
            if (entry != null)
                entity.GetDbEntityEntry().Collection(navigationPropertyName).Load();
        }

        public static void RemoveEntity<T>(this ICollection<T> collection, T entity)
            where T : Entity
        {
            collection.Remove(entity);
            entity.MarkAsDeleted();
        }

        public static void ClearEntities<T>(this ICollection<T> collection)
            where T : Entity
        {
            collection?.ToList()
                .ForEach(collection.RemoveEntity);
        }
    }
}