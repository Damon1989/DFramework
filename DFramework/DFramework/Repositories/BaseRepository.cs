namespace DFramework.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using DFramework.Specifications;

    /// <summary>
    ///     Represents the base class for repositories.
    /// </summary>
    /// <typeparam name="TAggregateRoot">The type of the aggregate root.</typeparam>
    public abstract class BaseRepository<TAggregateRoot> : IRepository<TAggregateRoot>
        where TAggregateRoot : class
    {
        public void Add(IEnumerable<TAggregateRoot> entities)
        {
            this.DoAdd(entities);
        }

        public void Add(TAggregateRoot entity)
        {
            this.DoAdd(entity);
        }

        public long Count(ISpecification<TAggregateRoot> specification)
        {
            return this.DoCount(specification);
        }

        public long Count(Expression<Func<TAggregateRoot, bool>> specification)
        {
            return this.DoCount(specification);
        }

        public Task<long> CountAsync(ISpecification<TAggregateRoot> specification)
        {
            return this.DoCountAsync(specification);
        }

        public Task<long> CountAsync(Expression<Func<TAggregateRoot, bool>> specification)
        {
            return this.DoCountAsync(specification);
        }

        public bool Exists(ISpecification<TAggregateRoot> specification)
        {
            return this.DoExists(specification);
        }

        public bool Exists(Expression<Func<TAggregateRoot, bool>> specification)
        {
            return this.DoExists(Specification<TAggregateRoot>.Eval(specification));
        }

        public Task<bool> ExistsAsync(ISpecification<TAggregateRoot> specification)
        {
            return this.DoExistsAsync(specification);
        }

        public Task<bool> ExistsAsync(Expression<Func<TAggregateRoot, bool>> specification)
        {
            return this.DoExistsAsync(Specification<TAggregateRoot>.Eval(specification));
        }

        public TAggregateRoot Find(ISpecification<TAggregateRoot> specification)
        {
            return this.DoFind(specification);
        }

        public TAggregateRoot Find(Expression<Func<TAggregateRoot, bool>> specification)
        {
            return this.DoFind(Specification<TAggregateRoot>.Eval(specification));
        }

        public IQueryable<TAggregateRoot> FindAll(params OrderExpression[] orderExpressions)
        {
            return this.DoFindAll(orderExpressions);
        }

        public IQueryable<TAggregateRoot> FindAll(
            ISpecification<TAggregateRoot> specification,
            params OrderExpression[] orderExpressions)
        {
            return this.DoFindAll(specification, orderExpressions);
        }

        public IQueryable<TAggregateRoot> FindAll(
            Expression<Func<TAggregateRoot, bool>> specification,
            params OrderExpression[] orderExpressions)
        {
            return this.DoFindAll(Specification<TAggregateRoot>.Eval(specification));
        }

        public Task<TAggregateRoot> FindAsync(ISpecification<TAggregateRoot> specification)
        {
            return this.DoFindAsync(specification);
        }

        public Task<TAggregateRoot> FindAsync(Expression<Func<TAggregateRoot, bool>> specification)
        {
            return this.DoFindAsync(Specification<TAggregateRoot>.Eval(specification));
        }

        public TAggregateRoot GetByKey(params object[] keyValues)
        {
            return this.DoGetByKey(keyValues);
        }

        public Task<TAggregateRoot> GetByKeyAsync(params object[] keyValues)
        {
            return this.DoGetByKeyAsync(keyValues);
        }

        public IQueryable<TAggregateRoot> PageFind(
            int pageIndex,
            int pageSize,
            Expression<Func<TAggregateRoot, bool>> expression,
            params OrderExpression[] orderExpressions)
        {
            return this.DoPageFind(
                pageIndex,
                pageSize,
                Specification<TAggregateRoot>.Eval(expression),
                orderExpressions);
        }

        public IQueryable<TAggregateRoot> PageFind(
            int pageIndex,
            int pageSize,
            Expression<Func<TAggregateRoot, bool>> expression,
            ref long totalCount,
            params OrderExpression[] orderExpressions)
        {
            return this.DoPageFind(
                pageIndex,
                pageSize,
                Specification<TAggregateRoot>.Eval(expression),
                ref totalCount,
                orderExpressions);
        }

        public IQueryable<TAggregateRoot> PageFind(
            int pageIndex,
            int pageSize,
            ISpecification<TAggregateRoot> specification,
            params OrderExpression[] orderbyExpressions)
        {
            return this.DoPageFind(pageIndex, pageSize, specification, orderbyExpressions);
        }

        public IQueryable<TAggregateRoot> PageFind(
            int pageIndex,
            int pageSize,
            ISpecification<TAggregateRoot> speccifiaction,
            ref long totalCount,
            params OrderExpression[] orderByExpressions)
        {
            return this.DoPageFind(pageIndex, pageSize, speccifiaction, ref totalCount, orderByExpressions);
        }

        public Task<Tuple<IQueryable<TAggregateRoot>, long>> PageFindAsync(
            int pageIndex,
            int pageSize,
            Expression<Func<TAggregateRoot, bool>> expression,
            params OrderExpression[] orderExpressions)
        {
            return this.DoPageFindAsync(
                pageIndex,
                pageSize,
                Specification<TAggregateRoot>.Eval(expression),
                orderExpressions);
        }

        public Task<Tuple<IQueryable<TAggregateRoot>, long>> PageFindAsync(
            int pageIndex,
            int pageSize,
            ISpecification<TAggregateRoot> specification,
            params OrderExpression[] orderExpressions)
        {
            return this.DoPageFindAsync(pageIndex, pageSize, specification, orderExpressions);
        }

        public void Reload(TAggregateRoot entity)
        {
            this.DoReload(entity);
        }

        public Task ReloadAsync(TAggregateRoot entity)
        {
            return this.DoReloadAsync(entity);
        }

        public void Remove(TAggregateRoot entity)
        {
            this.DoRemove(entity);
        }

        public void Remove(IEnumerable<TAggregateRoot> entities)
        {
            foreach (var entity in entities) this.DoRemove(entity);
        }

        public void Update(TAggregateRoot entity)
        {
            this.DoUpdate(entity);
        }

        protected abstract void DoAdd(IEnumerable<TAggregateRoot> entities);

        protected abstract void DoAdd(TAggregateRoot entity);

        protected abstract long DoCount(ISpecification<TAggregateRoot> specification);

        protected abstract long DoCount(Expression<Func<TAggregateRoot, bool>> specification);

        protected abstract Task<long> DoCountAsync(ISpecification<TAggregateRoot> specification);

        protected abstract Task<long> DoCountAsync(Expression<Func<TAggregateRoot, bool>> specification);

        protected abstract bool DoExists(ISpecification<TAggregateRoot> specification);

        protected abstract Task<bool> DoExistsAsync(ISpecification<TAggregateRoot> specification);

        protected abstract TAggregateRoot DoFind(ISpecification<TAggregateRoot> specification);

        protected virtual IQueryable<TAggregateRoot> DoFindAll(params OrderExpression[] orderExpressions)
        {
            return this.DoFindAll(new AllSpecification<TAggregateRoot>(), orderExpressions);
        }

        protected abstract IQueryable<TAggregateRoot> DoFindAll(
            ISpecification<TAggregateRoot> specification,
            params OrderExpression[] orderExpressions);

        protected abstract Task<TAggregateRoot> DoFindAsync(ISpecification<TAggregateRoot> specification);

        protected abstract TAggregateRoot DoGetByKey(params object[] keyValues);

        protected abstract Task<TAggregateRoot> DoGetByKeyAsync(params object[] keyValues);

        protected abstract IQueryable<TAggregateRoot> DoPageFind(
            int pageIndex,
            int pageSize,
            ISpecification<TAggregateRoot> specification,
            ref long totalCount,
            params OrderExpression[] orderExpressions);

        protected abstract IQueryable<TAggregateRoot> DoPageFind(
            int pageIndex,
            int pageSize,
            ISpecification<TAggregateRoot> specification,
            params OrderExpression[] orderExpressions);

        protected abstract Task<Tuple<IQueryable<TAggregateRoot>, long>> DoPageFindAsync(
            int pageIndex,
            int pageSize,
            ISpecification<TAggregateRoot> specification,
            params OrderExpression[] orderExpressions);

        protected abstract void DoReload(TAggregateRoot entity);

        protected abstract Task DoReloadAsync(TAggregateRoot entity);

        protected abstract void DoRemove(TAggregateRoot entity);

        protected abstract void DoUpdate(TAggregateRoot entity);
    }
}