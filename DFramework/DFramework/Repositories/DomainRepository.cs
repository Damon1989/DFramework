namespace DFramework.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using DFramework.Domain;
    using DFramework.IoC;
    using DFramework.Specifications;
    using DFramework.UnitOfWork;

    public class DomainRepository : IDomainRepository
    {
        protected object DbContext;

        private readonly IContainer _container;

        private readonly Dictionary<Type, IRepository> _repositories;

        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        ///     Initializes a new instance of DomainRepository.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="container"></param>
        public DomainRepository(object dbContext, IUnitOfWork unitOfWork, IContainer container)
        {
            this.DbContext = dbContext;
            this._unitOfWork = unitOfWork;
            this._container = container;
            this._repositories = new Dictionary<Type, IRepository>();
        }

        public void Add<TAggregateRoot>(IEnumerable<TAggregateRoot> entities)
            where TAggregateRoot : class
        {
            this.GetRepository<TAggregateRoot>().Add(entities);
        }

        public void Add<TAggergateRoot>(TAggergateRoot entity)
            where TAggergateRoot : class
        {
            this.GetRepository<TAggergateRoot>().Add(entity);
        }

        public long Count<TAggergateRoot>(ISpecification<TAggergateRoot> specification)
            where TAggergateRoot : class
        {
            return this.GetRepository<TAggergateRoot>().Count(specification);
        }

        public long Count<TAggergateRoot>(Expression<Func<TAggergateRoot, bool>> specification)
            where TAggergateRoot : class
        {
            return this.GetRepository<TAggergateRoot>().Count(specification);
        }

        public Task<long> CountAsync<TAggergateRoot>(ISpecification<TAggergateRoot> specification)
            where TAggergateRoot : class
        {
            return this.GetRepository<TAggergateRoot>().CountAsync(specification);
        }

        public Task<long> CountAsync<TAggergateRoot>(Expression<Func<TAggergateRoot, bool>> specification)
            where TAggergateRoot : class
        {
            return this.GetRepository<TAggergateRoot>().CountAsync(specification);
        }

        public bool Exists<TAggregateRoot>(ISpecification<TAggregateRoot> specification)
            where TAggregateRoot : class
        {
            return this.GetRepository<TAggregateRoot>().Exists(specification);
        }

        public bool Exists<TAggregateRoot>(Expression<Func<TAggregateRoot, bool>> specification)
            where TAggregateRoot : class
        {
            return this.GetRepository<TAggregateRoot>().Exists(specification);
        }

        public Task<bool> ExistsAsync<TAggregateRoot>(ISpecification<TAggregateRoot> specification)
            where TAggregateRoot : class
        {
            return this.GetRepository<TAggregateRoot>().ExistsAsync(specification);
        }

        public Task<bool> ExistsAsync<TAggregateRoot>(Expression<Func<TAggregateRoot, bool>> specification)
            where TAggregateRoot : class
        {
            return this.GetRepository<TAggregateRoot>().ExistsAsync(specification);
        }

        public TAggergateRoot Find<TAggergateRoot>(ISpecification<TAggergateRoot> specification)
            where TAggergateRoot : class
        {
            return this.GetRepository<TAggergateRoot>().Find(specification);
        }

        public TAggergateRoot Find<TAggergateRoot>(Expression<Func<TAggergateRoot, bool>> specification)
            where TAggergateRoot : class
        {
            return this.GetRepository<TAggergateRoot>().Find(specification);
        }

        public IQueryable<TAggergateRoot> FindAll<TAggergateRoot>(params OrderExpression[] orderExpressions)
            where TAggergateRoot : class
        {
            return this.GetRepository<TAggergateRoot>().FindAll(orderExpressions);
        }

        public IQueryable<TAggergateRoot> FindAll<TAggergateRoot>(
            ISpecification<TAggergateRoot> specification,
            params OrderExpression[] orderExpressions)
            where TAggergateRoot : class
        {
            return this.GetRepository<TAggergateRoot>().FindAll(specification, orderExpressions);
        }

        public IQueryable<TAggergateRoot> FindAll<TAggergateRoot>(
            Expression<Func<TAggergateRoot, bool>> specification,
            params OrderExpression[] orderExpressions)
            where TAggergateRoot : class
        {
            return this.GetRepository<TAggergateRoot>().FindAll(specification, orderExpressions);
        }

        public Task<TAggergateRoot> FindAsync<TAggergateRoot>(ISpecification<TAggergateRoot> specification)
            where TAggergateRoot : class
        {
            return this.GetRepository<TAggergateRoot>().FindAsync(specification);
        }

        public Task<TAggergateRoot> FindAsync<TAggergateRoot>(Expression<Func<TAggergateRoot, bool>> specification)
            where TAggergateRoot : class
        {
            return this.GetRepository<TAggergateRoot>().FindAsync(specification);
        }

        public TAggergateRoot GetByKey<TAggergateRoot>(params object[] keyValue)
            where TAggergateRoot : class
        {
            return this.GetRepository<TAggergateRoot>().GetByKey(keyValue);
        }

        public Task<TAggergateRoot> GetByKeyAsync<TAggergateRoot>(params object[] keyValues)
            where TAggergateRoot : class
        {
            return this.GetRepository<TAggergateRoot>().GetByKeyAsync(keyValues);
        }

        public IRepository<TAggergateRoot> GetRepository<TAggergateRoot>()
            where TAggergateRoot : class
        {
            if (this._repositories.TryGetValue(typeof(IRepository<TAggergateRoot>), out var repository))
                return repository as IRepository<TAggergateRoot>;

            repository = this._container.Resolve<IRepository<TAggergateRoot>>(
                new Parameter("dbContext", this.DbContext),
                new Parameter("unitOfWork", this._unitOfWork));
            this._repositories.Add(typeof(IRepository<TAggergateRoot>), repository);

            return repository as IRepository<TAggergateRoot>;
        }

        public IQueryable<TAggregateRoot> PageFind<TAggregateRoot>(
            int pageIndex,
            int pageSize,
            Expression<Func<TAggregateRoot, bool>> specification,
            params OrderExpression[] orderExpressions)
            where TAggregateRoot : class
        {
            return this.GetRepository<TAggregateRoot>().PageFind(pageIndex, pageSize, specification, orderExpressions);
        }

        public IQueryable<TAggregateRoot> PageFind<TAggregateRoot>(
            int pageIndex,
            int pageSize,
            Expression<Func<TAggregateRoot, bool>> specification,
            ref long totalCount,
            params OrderExpression[] orderExpressions)
            where TAggregateRoot : class
        {
            return this.GetRepository<TAggregateRoot>().PageFind(
                pageIndex,
                pageSize,
                specification,
                ref totalCount,
                orderExpressions);
        }

        public IQueryable<TAggregateRoot> PageFind<TAggregateRoot>(
            int pageIndex,
            int pageSize,
            ISpecification<TAggregateRoot> specification,
            ref long totalCount,
            params OrderExpression[] orderExpressions)
            where TAggregateRoot : class
        {
            return this.GetRepository<TAggregateRoot>().PageFind(
                pageIndex,
                pageSize,
                specification,
                ref totalCount,
                orderExpressions);
        }

        public IQueryable<TAggregateRoot> PageFind<TAggregateRoot>(
            int pageIndex,
            int pageSize,
            ISpecification<TAggregateRoot> specification,
            params OrderExpression[] orderExpressions)
            where TAggregateRoot : class
        {
            return this.GetRepository<TAggregateRoot>().PageFind(pageIndex, pageSize, specification, orderExpressions);
        }

        public Task<Tuple<IQueryable<TAggregateRoot>, long>> PageFindAsync<TAggregateRoot>(
            int pageIndex,
            int pageSize,
            Expression<Func<TAggregateRoot, bool>> specification,
            params OrderExpression[] orderExpressions)
            where TAggregateRoot : class
        {
            return this.GetRepository<TAggregateRoot>().PageFindAsync(
                pageIndex,
                pageSize,
                specification,
                orderExpressions);
        }

        public Task<Tuple<IQueryable<TAggregateRoot>, long>> PageFindAsync<TAggregateRoot>(
            int pageIndex,
            int pageSize,
            ISpecification<TAggregateRoot> specification,
            params OrderExpression[] orderExpressions)
            where TAggregateRoot : class
        {
            return this.GetRepository<TAggregateRoot>().PageFindAsync(
                pageIndex,
                pageSize,
                specification,
                orderExpressions);
        }

        public void Reload<TEntity>(TEntity entity)
            where TEntity : Entity
        {
            this.GetRepository<TEntity>().Reload(entity);
        }

        public Task ReloadAsync<TEntity>(TEntity entity)
            where TEntity : Entity
        {
            return this.GetRepository<TEntity>().ReloadAsync(entity);
        }

        public void Remove<TAggregateRoot>(TAggregateRoot entity)
            where TAggregateRoot : class
        {
            this.GetRepository<TAggregateRoot>().Remove(entity);
        }

        public void Remove<TAggregateRoot>(IEnumerable<TAggregateRoot> entities)
            where TAggregateRoot : class
        {
            this.GetRepository<TAggregateRoot>().Remove(entities);
        }

        public void Update<TAggregateRoot>(TAggregateRoot entity)
            where TAggregateRoot : class
        {
            this.GetRepository<TAggregateRoot>().Update(entity);
        }
    }
}