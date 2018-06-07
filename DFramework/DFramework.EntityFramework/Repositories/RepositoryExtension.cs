using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DFramework.Repositories;

namespace DFramework.EntityFramework.Repositories
{
    public static class RepositoryExtension
    {
        public static void AddWithSaveChanges<TEntity>(this IRepository<TEntity> repository, TEntity entity)
             where TEntity : class
        {
            repository.Add(entity);
            (repository as Repository<TEntity>).Container.SaveChanges();
        }

        public static void AddWithSaveChanges<TEntity>(this IDomainRepository repository, TEntity entity)
        where TEntity : class
        {
            (repository as DomainRepository).GetRepository<TEntity>().AddWithSaveChanges(entity);
        }

        public static IQueryable Include(this IQueryable source, string path)
        {
            return QueryableExtensions.Include(source, path);
        }

        public static IQueryable<T> Include<T>(this IQueryable<T> source, string path)
        {
            return QueryableExtensions.Include(source, path);
        }

        public static IQueryable<T> Include<T, TProperty>(this IQueryable<T> source,
            Expression<Func<T, TProperty>> path)
        {
            return QueryableExtensions.Include(source, path);
        }

        public static Task<List<TEntity>> ToListAsync<TEntity>(this IQueryable<TEntity> query) where TEntity : class
        {
            return QueryableExtensions.ToListAsync(query);
        }

        public static Task<TEntity> FirstOrDefaultAsync<TEntity>(this IQueryable<TEntity> query,
            Expression<Func<TEntity, bool>> predicate = null) where TEntity : class
        {
            if (predicate == null)
            {
                return QueryableExtensions.FirstOrDefaultAsync(query);
            }
            return QueryableExtensions.FirstOrDefaultAsync(query, predicate);
        }
    }
}