using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DFramework.Infrastructure;
using DFramework.Repositories;
using DFramework.Specifications;
using DFramework.UnitOfWork;

namespace DFramework.EntityFramework.Repositories
{
    public class Repository<TEntity> : BaseRepository<TEntity>, IMergeOptionChangable
        where TEntity : class
    {
        internal DbContext Container;
        private DbSet<TEntity> _objectSet;

        public Repository(MSDbContext dbContext, IUnitOfWork unitOfWork)
        {
            if (dbContext == null)
            {
                throw new Exception("repository could not work without dbContext");
            }
            (unitOfWork as UnitOfWork)?.RegisterDbContext(dbContext);
            Container = dbContext;
        }

        private DbSet<TEntity> DbSet => _objectSet ?? (_objectSet = Container.Set<TEntity>());

        public void ChangeMergeOption<TMergeOptionEntity>(MergeOption mergeOption) where TMergeOptionEntity : class
        {
            var objectContext = ((IObjectContextAdapter)Container).ObjectContext;
            var set = objectContext.CreateObjectSet<TMergeOptionEntity>();
            set.MergeOption = mergeOption;
        }

        protected override TEntity DoGetByKey(params object[] keyValues)
        {
            return DbSet.Find(keyValues);
        }

        protected override Task<TEntity> DoGetByKeyAsync(params object[] keyValues)
        {
            return DbSet.FindAsync(keyValues);
        }

        protected override IQueryable<TEntity> DoFindAll(ISpecification<TEntity> specification,
            params OrderExpression[] orderExpressions)
        {
            return DbSet.FindAll(specification, orderExpressions);
        }

        protected override void DoRemove(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        protected override void DoUpdate(TEntity entity)
        {
            Container.Entry(entity).State = EntityState.Modified;
        }

        protected override IQueryable<TEntity> DoPageFind(int pageIndex, int pageSize,
            ISpecification<TEntity> specification, ref long totalCount,
            params OrderExpression[] orderExpressions)
        {
            throw new NotImplementedException();
        }

        protected override Task<Tuple<IQueryable<TEntity>, long>> DoPageFindAsync(int pageIndex, int pageSize, ISpecification<TEntity> specification,
            params OrderExpression[] orderExpressions)
        {
            throw new NotImplementedException();
        }

        protected override IQueryable<TEntity> DoPageFind(int pageIndex, int pageSize,
            ISpecification<TEntity> specification,
            params OrderExpression[] orderExpressions)
        {
            if (pageIndex < 0)
            {
                throw new ArgumentException("InvalidPageIndex");
            }

            if (pageSize <= 0)
            {
                throw new ArgumentException("InvalidPageCount");
            }

            if (orderExpressions == null || orderExpressions.Length == 0)
            {
                throw new ArgumentNullException("OrderByExpressionCannotBeNull");
            }

            if (specification == null)
            {
                specification = new AllSpecification<TEntity>();
            }

            var query = DoFindAll(specification, orderExpressions);
            return query.GetPageElements(pageIndex, pageSize);
        }

        protected override void DoAdd(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        protected override void DoAdd(TEntity entity)
        {
            throw new NotImplementedException();
        }

        protected override long DoCount(ISpecification<TEntity> specification)
        {
            throw new NotImplementedException();
        }

        protected override Task<long> DoCountAsync(ISpecification<TEntity> specification)
        {
            throw new NotImplementedException();
        }

        protected override long DoCount(Expression<Func<TEntity, bool>> specification)
        {
            throw new NotImplementedException();
        }

        protected override Task<long> DoCountAsync(Expression<Func<TEntity, bool>> specification)
        {
            throw new NotImplementedException();
        }

        protected override bool DoExists(ISpecification<TEntity> specification)
        {
            throw new NotImplementedException();
        }

        protected override Task<bool> DoExistsAsync(ISpecification<TEntity> specification)
        {
            throw new NotImplementedException();
        }

        protected override TEntity DoFind(ISpecification<TEntity> specification)
        {
            throw new NotImplementedException();
        }

        protected override Task<TEntity> DoFindAsync(ISpecification<TEntity> specification)
        {
            throw new NotImplementedException();
        }

        protected override void DoReload(TEntity entity)
        {
            throw new NotImplementedException();
        }

        protected override Task DoReloadAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}