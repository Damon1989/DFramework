using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DFramework.Domain;
using DFramework.Specifications;

namespace DFramework.Repositories
{
    public interface IDomainRepository
    {
        void Add<TAggregateRoot>(IEnumerable<TAggregateRoot> entities) where TAggregateRoot : class;

        void Add<TaggergateRoot>(TaggergateRoot entity) where TaggergateRoot : class;

        TAggergateRoot GetByKey<TAggergateRoot>(params object[] keyValue) where TAggergateRoot : class;

        Task<TAggergateRoot> GetByKeyAsync<TAggergateRoot>(params object[] keyValues) where TAggergateRoot : class;

        long Count<TAggergateRoot>(ISpecification<TAggergateRoot> specification) where TAggergateRoot : class;

        Task<long> CountAsync<TAggergateRoot>(ISpecification<TAggergateRoot> specification)
            where TAggergateRoot : class;

        long Count<TAggergateRoot>(Expression<Func<TAggergateRoot, bool>> specification) where TAggergateRoot : class;

        Task<long> CountAsync<TAggergateRoot>(Expression<Func<TAggergateRoot, bool>> specification)
            where TAggergateRoot : class;

        void Reload<TEntity>(TEntity entity) where TEntity : Entity;

        Task ReloadAsync<TEntity>(TEntity entity) where TEntity : Entity;

        IQueryable<TAggergateRoot> FindAll<TAggergateRoot>(params OrderExpression[] orderExpressions)
            where TAggergateRoot : class;

        IQueryable<TAggergateRoot> FindAll<TAggergateRoot>(ISpecification<TAggergateRoot> specification,
            params OrderExpression[] orderExpressions) where TAggergateRoot : class;

        IQueryable<TAggergateRoot> FindAll<TAggergateRoot>(Expression<Func<TAggergateRoot, bool>> specification,
            params OrderExpression[] orderExpressions) where TAggergateRoot : class;

        TAggergateRoot Find<TAggergateRoot>(ISpecification<TAggergateRoot> specification)
            where TAggergateRoot : class;

        Task<TAggergateRoot> FindAsync<TAggergateRoot>(ISpecification<TAggergateRoot> specification)
            where TAggergateRoot : class;

        TAggergateRoot Find<TAggergateRoot>(Expression<Func<TAggergateRoot, bool>> specification)
            where TAggergateRoot : class;

        Task<TAggergateRoot> FindAsync<TAggergateRoot>(Expression<Func<TAggergateRoot, bool>> specification)
            where TAggergateRoot : class;

        bool Exists<TAggregateRoot>(ISpecification<TAggregateRoot> specification)
            where TAggregateRoot : class;

        Task<bool> ExistsAsync<TAggregateRoot>(ISpecification<TAggregateRoot> specification)
            where TAggregateRoot : class;

        bool Exists<TAggregateRoot>(Expression<Func<TAggregateRoot, bool>> specification)
            where TAggregateRoot : class;

        Task<bool> ExistsAsync<TAggregateRoot>(Expression<Func<TAggregateRoot, bool>> specification)
            where TAggregateRoot : class;

        void Remove<TAggregateRoot>(TAggregateRoot entity) where TAggregateRoot : class;

        void Remove<TAggregateRoot>(IEnumerable<TAggregateRoot> entities) where TAggregateRoot : class;

        void Update<TAggregateRoot>(TAggregateRoot entity) where TAggregateRoot : class;

        IQueryable<TAggregateRoot> PageFind<TAggregateRoot>(int pageIndex, int pageSize,
            Expression<Func<TAggregateRoot, bool>> specification, params OrderExpression[] orderExpressions)
            where TAggregateRoot : class;

        IQueryable<TAggregateRoot> PageFind<TAggregateRoot>(int pageIndex, int pageSize,
            Expression<Func<TAggregateRoot, bool>> specification, ref long totalCount,
            params OrderExpression[] orderExpressions) where TAggregateRoot : class;

        Task<Tuple<IQueryable<TAggregateRoot>, long>> PageFindAsync<TAggregateRoot>(int pageIndex, int pageSize,
            Expression<Func<TAggregateRoot, bool>> specification, params OrderExpression[] orderExpressions)
            where TAggregateRoot : class;

        IQueryable<TAggregateRoot> PageFind<TAggregateRoot>(int pageIndex, int pageSize,
            ISpecification<TAggregateRoot> specification, ref long totalCount,
            params OrderExpression[] orderExpressions) where TAggregateRoot : class;

        Task<Tuple<IQueryable<TAggregateRoot>, long>> PageFindAsync<TAggregateRoot>(int pageIndex, int pageSize,
            ISpecification<TAggregateRoot> specification, params OrderExpression[] orderExpressions)
            where TAggregateRoot : class;

        IQueryable<TAggregateRoot> PageFind<TAggregateRoot>(int pageIndex, int pageSize,
            ISpecification<TAggregateRoot> specification, params OrderExpression[] orderExpressions)
            where TAggregateRoot : class;
    }
}