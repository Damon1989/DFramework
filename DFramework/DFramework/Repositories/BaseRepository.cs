using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DFramework.Specifications;

namespace DFramework.Repositories
{
    /// <summary>
    /// Represents the base class for repositories.
    /// </summary>
    /// <typeparam name="TAggregateRoot">The type of the aggregate root.</typeparam>
    public abstract class BaseRepository<TAggregateRoot> : IRepository<TAggregateRoot>
            where TAggregateRoot : class
    {
        #region Add

        protected abstract void DoAdd(IEnumerable<TAggregateRoot> entities);

        protected abstract void DoAdd(TAggregateRoot entity);

        public void Add(IEnumerable<TAggregateRoot> entities)
        {
            DoAdd(entities);
        }

        public void Add(TAggregateRoot entity)
        {
            DoAdd(entity);
        }

        #endregion Add

        #region Count

        protected abstract long DoCount(ISpecification<TAggregateRoot> specification);

        protected abstract Task<long> DoCountAsync(ISpecification<TAggregateRoot> specification);

        protected abstract long DoCount(Expression<Func<TAggregateRoot, bool>> specification);

        protected abstract Task<long> DoCountAsync(Expression<Func<TAggregateRoot, bool>> specification);

        public long Count(ISpecification<TAggregateRoot> specification)
        {
            return DoCount(specification);
        }

        public long Count(Expression<Func<TAggregateRoot, bool>> specification)
        {
            return DoCount(specification);
        }

        public Task<long> CountAsync(ISpecification<TAggregateRoot> specification)
        {
            return DoCountAsync(specification);
        }

        public Task<long> CountAsync(Expression<Func<TAggregateRoot, bool>> specification)
        {
            return DoCountAsync(specification);
        }

        #endregion Count

        #region Exists

        protected abstract bool DoExists(ISpecification<TAggregateRoot> specification);

        protected abstract Task<bool> DoExistsAsync(ISpecification<TAggregateRoot> specification);

        public bool Exists(ISpecification<TAggregateRoot> specification)
        {
            return DoExists(specification);
        }

        public bool Exists(Expression<Func<TAggregateRoot, bool>> specification)
        {
            return DoExists(Specification<TAggregateRoot>.Eval(specification));
        }

        public Task<bool> ExistsAsync(ISpecification<TAggregateRoot> specification)
        {
            return DoExistsAsync(specification);
        }

        public Task<bool> ExistsAsync(Expression<Func<TAggregateRoot, bool>> specification)
        {
            return DoExistsAsync(Specification<TAggregateRoot>.Eval(specification));
        }

        #endregion Exists

        #region Find

        protected abstract TAggregateRoot DoFind(ISpecification<TAggregateRoot> specification);

        protected abstract Task<TAggregateRoot> DoFindAsync(ISpecification<TAggregateRoot> specification);

        public TAggregateRoot Find(ISpecification<TAggregateRoot> specification)
        {
            return DoFind(specification);
        }

        public TAggregateRoot Find(Expression<Func<TAggregateRoot, bool>> specification)
        {
            return DoFind(Specification<TAggregateRoot>.Eval(specification));
        }

        public Task<TAggregateRoot> FindAsync(ISpecification<TAggregateRoot> specification)
        {
            return DoFindAsync(specification);
        }

        public Task<TAggregateRoot> FindAsync(Expression<Func<TAggregateRoot, bool>> specification)
        {
            return DoFindAsync(Specification<TAggregateRoot>.Eval(specification));
        }

        #endregion Find

        #region FindAll

        protected virtual IQueryable<TAggregateRoot> DoFindAll(params OrderExpression[] orderExpressions)
        {
            return DoFindAll(new AllSpecification<TAggregateRoot>(), orderExpressions);
        }

        protected abstract IQueryable<TAggregateRoot> DoFindAll(ISpecification<TAggregateRoot> specification,
            params OrderExpression[] orderExpressions);

        public IQueryable<TAggregateRoot> FindAll(params OrderExpression[] orderExpressions)
        {
            return DoFindAll(orderExpressions);
        }

        public IQueryable<TAggregateRoot> FindAll(ISpecification<TAggregateRoot> specification, params OrderExpression[] orderExpressions)
        {
            return DoFindAll(specification, orderExpressions);
        }

        public IQueryable<TAggregateRoot> FindAll(Expression<Func<TAggregateRoot, bool>> specification, params OrderExpression[] orderExpressions)
        {
            return DoFindAll(Specification<TAggregateRoot>.Eval(specification));
        }

        #endregion FindAll

        #region GetBy

        protected abstract TAggregateRoot DoGetByKey(params object[] keyValues);

        protected abstract Task<TAggregateRoot> DoGetByKeyAsync(params object[] keyValues);

        public TAggregateRoot GetByKey(params object[] keyValues)
        {
            return DoGetByKey(keyValues);
        }

        public Task<TAggregateRoot> GetByKeyAsync(params object[] keyValues)
        {
            return DoGetByKeyAsync(keyValues);
        }

        #endregion GetBy

        #region PageFind

        protected abstract IQueryable<TAggregateRoot> DoPageFind(int pageIndex,
            int pageSize,
            ISpecification<TAggregateRoot> specification,
            ref long totalCount,
            params OrderExpression[] orderExpressions);

        protected abstract Task<Tuple<IQueryable<TAggregateRoot>, long>> DoPageFindAsync(int pageIndex,
            int pageSize,
            ISpecification<TAggregateRoot> specification,
            params OrderExpression[] orderExpressions);

        protected abstract IQueryable<TAggregateRoot> DoPageFind(int pageIndex,
            int pageSize,
            ISpecification<TAggregateRoot> specification,
            params OrderExpression[] orderExpressions);

        public IQueryable<TAggregateRoot> PageFind(int pageIndex,
                                                    int pageSize,
                                                    Expression<Func<TAggregateRoot, bool>> expression,
                                                    params OrderExpression[] orderExpressions)
        {
            return DoPageFind(pageIndex, pageSize, Specification<TAggregateRoot>.Eval(expression), orderExpressions);
        }

        public IQueryable<TAggregateRoot> PageFind(int pageIndex,
                                                    int pageSize,
                                                    Expression<Func<TAggregateRoot, bool>> expression,
                                                    ref long totalCount,
                                                    params OrderExpression[] orderExpressions)
        {
            return DoPageFind(pageIndex, pageSize, Specification<TAggregateRoot>.Eval(expression), ref totalCount,
                orderExpressions);
        }

        public IQueryable<TAggregateRoot> PageFind(int pageIndex,
                                                    int pageSize,
                                                    ISpecification<TAggregateRoot> specification,
                                                    params OrderExpression[] orderbyExpressions)
        {
            return DoPageFind(pageIndex, pageSize, specification, orderbyExpressions);
        }

        public IQueryable<TAggregateRoot> PageFind(int pageIndex,
                                                    int pageSize,
                                                    ISpecification<TAggregateRoot> speccifiaction,
                                                    ref long totalCount,
                                                    params OrderExpression[] orderByExpressions)
        {
            return DoPageFind(pageIndex, pageSize, speccifiaction, ref totalCount, orderByExpressions);
        }

        public Task<Tuple<IQueryable<TAggregateRoot>, long>> PageFindAsync(int pageIndex,
                                                                            int pageSize,
                                                                            Expression<Func<TAggregateRoot, bool>> specification,
                                                                            params OrderExpression[] orderExpressions)
        {
            return DoPageFindAsync(pageIndex, pageSize, Specification<TAggregateRoot>.Eval(specification), orderExpressions);
        }

        public Task<Tuple<IQueryable<TAggregateRoot>, long>> PageFindAsync(int pageIndex,
                                                                            int pageSize,
                                                                            ISpecification<TAggregateRoot> specification,
                                                                            params OrderExpression[] orderExpressions)
        {
            return DoPageFindAsync(pageIndex, pageSize, specification, orderExpressions);
        }

        #endregion PageFind

        #region Reload

        protected abstract void DoReload(TAggregateRoot entity);

        protected abstract Task DoReloadAsync(TAggregateRoot entity);

        public void Reload(TAggregateRoot entity)
        {
            DoReload(entity);
        }

        public Task ReloadAsync(TAggregateRoot entity)
        {
            return DoReloadAsync(entity);
        }

        #endregion Reload

        #region Remove

        protected abstract void DoRemove(TAggregateRoot entity);

        public void Remove(TAggregateRoot entity)
        {
            DoRemove(entity);
        }

        public void Remove(IEnumerable<TAggregateRoot> entities)
        {
            foreach (var entity in entities)
            {
                DoRemove(entity);
            }
        }

        #endregion Remove

        #region update

        protected abstract void DoUpdate(TAggregateRoot entity);

        public void Update(TAggregateRoot entity)
        {
            DoUpdate(entity);
        }

        #endregion update
    }
}