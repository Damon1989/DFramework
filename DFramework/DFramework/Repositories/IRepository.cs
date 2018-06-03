using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DFramework.Specifications;

namespace DFramework.Repositories
{
    public interface IRepository
    {
    }

    /// <summary>
    ///  Represents the repositories
    /// </summary>
    /// <typeparam name="TAggergateRoot">The type of the aggregation root with which the repository is working.</typeparam>
    public interface IRepository<TAggergateRoot> : IRepository
        where TAggergateRoot : class
    {
        /// <summary>
        ///  Adds an entity to the repository.
        /// </summary>
        /// <param name="entities"></param>
        void Add(IEnumerable<TAggergateRoot> entities);

        void Add(TAggergateRoot entity);

        /// <summary>
        ///  Gets the entity instance from repository by a given key.
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        TAggergateRoot GetByKey(params object[] keyValues);

        Task<TAggergateRoot> GetByKeyAsync(params object[] keyValues);

        long Count(ISpecification<TAggergateRoot> specification);

        Task<long> CountAsync(ISpecification<TAggergateRoot> specification);

        long Count(Expression<Func<TAggergateRoot, bool>> specification);

        Task<long> CountAsync(Expression<Func<TAggergateRoot, bool>> specification);

        /// <summary>
        ///  Finds all the aggregate roots from repository,sorting by using the provided sort predicate
        ///  and the specified sort order.
        /// </summary>
        /// <param name="orderExpressions"></param>
        /// <returns>
        /// All the aggregate roots get from the repository,with the aggregate roots being sorted by using
        /// the provided sort predicate and the sort order.
        /// </returns>
        IQueryable<TAggergateRoot> FindAll(params OrderExpression[] orderExpressions);

        IQueryable<TAggergateRoot> FindAll(ISpecification<TAggergateRoot> specification,
                                            params OrderExpression[] orderExpressions);

        IQueryable<TAggergateRoot> FindAll(Expression<Func<TAggergateRoot, bool>> specification,
                                            params OrderExpression[] orderExpressions);

        /// <summary>
        ///  Finds a single aggregate root that matches the given specification
        /// </summary>
        /// <param name="specification">the specification with which the aggregate root should match.</param>
        /// <returns> The instance of the aggregate root.</returns>
        TAggergateRoot Find(ISpecification<TAggergateRoot> specification);

        Task<TAggergateRoot> FindAsync(ISpecification<TAggergateRoot> specification);

        TAggergateRoot Find(Expression<Func<TAggergateRoot, bool>> specification);

        Task<TAggergateRoot> FindAsync(Expression<Func<TAggergateRoot, bool>> specification);

        /// <summary>
        ///  Checks whether the aggregate root which matches the given specification exists.
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        bool Exists(ISpecification<TAggergateRoot> specification);

        Task<bool> ExistsAsync(ISpecification<TAggergateRoot> specification);

        bool Exists(Expression<Func<TAggergateRoot, bool>> specification);

        Task<bool> ExistsAsync(Expression<Func<TAggergateRoot, bool>> specification);

        /// <summary>
        ///  Removes the entity from the repository.
        /// </summary>
        /// <param name="entity"> The entity to be removed.</param>
        void Remove(TAggergateRoot entity);

        void Remove(IEnumerable<TAggergateRoot> entities);

        void Reload(TAggergateRoot entity);

        Task ReloadAsync(TAggergateRoot entity);

        /// <summary>
        ///  Updates the entity in the repository,
        /// </summary>
        /// <param name="entity">The eneity to be updated.</param>

        void Update(TAggergateRoot entity);

        IQueryable<TAggergateRoot> PageFind(int pageIndex,
                                            int pageSize,
                                            Expression<Func<TAggergateRoot, bool>> expression,
                                            params OrderExpression[] orderExpressions);

        IQueryable<TAggergateRoot> PageFind(int pageIndex,
                                            int pageSize,
                                            Expression<Func<TAggergateRoot, bool>> expression,
                                            ref long totalCount,
                                            params OrderExpression[] orderExpressions);

        Task<Tuple<IQueryable<TAggergateRoot>, long>> PageFindAsync(int pageIndex,
                                                                    int pageSize,
                                                                    Expression<Func<TAggergateRoot, bool>> specification,
                                                                    params OrderExpression[] orderExpressions);

        IQueryable<TAggergateRoot> PageFind(int pageIndex,
                                            int pageSize,
                                            ISpecification<TAggergateRoot> specification,
                                            params OrderExpression[] orderbyExpressions);

        IQueryable<TAggergateRoot> PageFind(int pageIndex, int pageSize,
                                            ISpecification<TAggergateRoot> speccifiaction,
                                            ref long totalCount,
                                            params OrderExpression[] orderByExpressions);

        Task<Tuple<IQueryable<TAggergateRoot>, long>> PageFindAsync(int pageIndex,
                                                                    int pageSize,
                                                                    ISpecification<TAggergateRoot> specification,
                                                                    params OrderExpression[] orderExpressions);
    }
}