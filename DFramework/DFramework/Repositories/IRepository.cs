namespace DFramework.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using DFramework.Specifications;

    public interface IRepository
    {
    }

    /// <summary>
    ///     Represents the repositories
    /// </summary>
    /// <typeparam name="TAggregateRoot">The type of the aggregation root with which the repository is working.</typeparam>
    public interface IRepository<TAggregateRoot> : IRepository
        where TAggregateRoot : class
    {
        /// <summary>
        ///     Adds an entity to the repository.
        /// </summary>
        /// <param name="entities"></param>
        void Add(IEnumerable<TAggregateRoot> entities);

        void Add(TAggregateRoot entity);

        long Count(ISpecification<TAggregateRoot> specification);

        long Count(Expression<Func<TAggregateRoot, bool>> expression);

        Task<long> CountAsync(ISpecification<TAggregateRoot> specification);

        Task<long> CountAsync(Expression<Func<TAggregateRoot, bool>> expression);

        /// <summary>
        ///     Checks whether the aggregate root which matches the given specification exists.
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        bool Exists(ISpecification<TAggregateRoot> specification);

        bool Exists(Expression<Func<TAggregateRoot, bool>> expression);

        Task<bool> ExistsAsync(ISpecification<TAggregateRoot> specification);

        Task<bool> ExistsAsync(Expression<Func<TAggregateRoot, bool>> expression);

        /// <summary>
        ///     Finds a single aggregate root that matches the given specification
        /// </summary>
        /// <param name="specification">the specification with which the aggregate root should match.</param>
        /// <returns> The instance of the aggregate root.</returns>
        TAggregateRoot Find(ISpecification<TAggregateRoot> specification);

        TAggregateRoot Find(Expression<Func<TAggregateRoot, bool>> specification);

        /// <summary>
        ///     Finds all the aggregate roots from repository,sorting by using the provided sort predicate
        ///     and the specified sort order.
        /// </summary>
        /// <param name="orderExpressions"></param>
        /// <returns>
        ///     All the aggregate roots get from the repository,with the aggregate roots being sorted by using
        ///     the provided sort predicate and the sort order.
        /// </returns>
        IQueryable<TAggregateRoot> FindAll(params OrderExpression[] orderExpressions);

        IQueryable<TAggregateRoot> FindAll(
            ISpecification<TAggregateRoot> specification,
            params OrderExpression[] orderExpressions);

        IQueryable<TAggregateRoot> FindAll(
            Expression<Func<TAggregateRoot, bool>> expression,
            params OrderExpression[] orderExpressions);

        Task<TAggregateRoot> FindAsync(ISpecification<TAggregateRoot> specification);

        Task<TAggregateRoot> FindAsync(Expression<Func<TAggregateRoot, bool>> expression);

        /// <summary>
        ///     Gets the entity instance from repository by a given key.
        /// </summary>
        /// <param name="keyValues">The key of the entity</param>
        /// <returns></returns>
        TAggregateRoot GetByKey(params object[] keyValues);

        Task<TAggregateRoot> GetByKeyAsync(params object[] keyValues);

        IQueryable<TAggregateRoot> PageFind(
            int pageIndex,
            int pageSize,
            Expression<Func<TAggregateRoot, bool>> expression,
            params OrderExpression[] orderExpressions);

        IQueryable<TAggregateRoot> PageFind(
            int pageIndex,
            int pageSize,
            Expression<Func<TAggregateRoot, bool>> expression,
            ref long totalCount,
            params OrderExpression[] orderExpressions);

        IQueryable<TAggregateRoot> PageFind(
            int pageIndex,
            int pageSize,
            ISpecification<TAggregateRoot> specification,
            params OrderExpression[] orderbyExpressions);

        IQueryable<TAggregateRoot> PageFind(
            int pageIndex,
            int pageSize,
            ISpecification<TAggregateRoot> speccifiaction,
            ref long totalCount,
            params OrderExpression[] orderByExpressions);

        Task<Tuple<IQueryable<TAggregateRoot>, long>> PageFindAsync(
            int pageIndex,
            int pageSize,
            Expression<Func<TAggregateRoot, bool>> expression,
            params OrderExpression[] orderExpressions);

        Task<Tuple<IQueryable<TAggregateRoot>, long>> PageFindAsync(
            int pageIndex,
            int pageSize,
            ISpecification<TAggregateRoot> specification,
            params OrderExpression[] orderExpressions);

        void Reload(TAggregateRoot entity);

        Task ReloadAsync(TAggregateRoot entity);

        /// <summary>
        ///     Removes the entity from the repository.
        /// </summary>
        /// <param name="entity"> The entity to be removed.</param>
        void Remove(TAggregateRoot entity);

        void Remove(IEnumerable<TAggregateRoot> entities);

        /// <summary>
        ///     Updates the entity in the repository,
        /// </summary>
        /// <param name="entity">The eneity to be updated.</param>
        void Update(TAggregateRoot entity);
    }
}