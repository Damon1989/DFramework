using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DFramework.Specifications;

namespace DFramework.Repositories
{
    public abstract class BaseRepository<TAggregateRoot> : IRepository<TAggregateRoot>
            where TAggregateRoot : class
    {
        protected abstract TAggregateRoot DoGetByKey(params object[] keyValues);

        protected abstract Task<TAggregateRoot> DoGetByKeyAsync(params object[] keyValues);

        protected virtual IQueryable<TAggregateRoot> DoFindAll(params OrderExpression[] orderExpressions)
        {
            return DoFindAll(new AllSpecification<TAggregateRoot>(), orderExpressions);
        }

        protected abstract IQueryable<TAggregateRoot> DoFindAll(ISpecification<TAggregateRoot> specification,
            params OrderExpression[] orderExpressions);

        protected abstract void DoRemove(TAggregateRoot entity);

        protected abstract void DoUpdate(TAggregateRoot entity);

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

        public Task<TAggregateRoot> FindAsync(ISpecification<TAggregateRoot> specification)
        {
            return DoFindAsync(specification);
        }

        public Task<TAggregateRoot> FindAsync(Expression<Func<TAggregateRoot, bool>> specification)
        {
            return DoFindAsync(Specification<TAggregateRoot>.Eval(specification));
        }

        public TAggregateRoot GetByKey(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public Task<TAggregateRoot> GetByKeyAsync(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TAggregateRoot> PageFind(int pageIndex, int pageSize, Expression<Func<TAggregateRoot, bool>> expression, params OrderExpression[] orderExpressions)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TAggregateRoot> PageFind(int pageIndex, int pageSize, Expression<Func<TAggregateRoot, bool>> expression, ref long totalCount, params OrderExpression[] orderExpressions)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TAggregateRoot> PageFind(int pageIndex, int pageSize, ISpecification<TAggregateRoot> specification, params OrderExpression[] orderbyExpressions)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TAggregateRoot> PageFind(int pageIndex, int pageSize, ISpecification<TAggregateRoot> speccifiaction, ref long totalCount, params OrderExpression[] orderByExpressions)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<IQueryable<TAggregateRoot>, long>> PageFindAsync(int pageIndex, int pageSize, Expression<Func<TAggregateRoot, bool>> specification, params OrderExpression[] orderExpressions)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<IQueryable<TAggregateRoot>, long>> PageFindAsync(int pageIndex, int pageSize, ISpecification<TAggregateRoot> specification, params OrderExpression[] orderExpressions)
        {
            throw new NotImplementedException();
        }

        public void Reload(TAggregateRoot entity)
        {
            throw new NotImplementedException();
        }

        public Task ReloadAsync(TAggregateRoot entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(TAggregateRoot entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(IEnumerable<TAggregateRoot> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(TAggregateRoot entity)
        {
            throw new NotImplementedException();
        }
    }
}