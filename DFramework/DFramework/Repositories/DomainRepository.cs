using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DFramework.Domain;
using DFramework.IoC;
using DFramework.Specifications;
using DFramework.UnitOfWork;

namespace DFramework.Repositories
{
    public class DomainRepository : IDomainRepository
    {
        private readonly IContainer Container;
        protected object DbContext;
        private readonly Dictionary<Type, IRepository> Repositories;
        private readonly IUnitOfWork UnitOfWork;

        /// <summary>
        ///  Initializes a new instance of DomainRepository.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="container"></param>
        public DomainRepository(object dbContext, IUnitOfWork unitOfWork, IContainer container)
        {
            DbContext = dbContext;
            UnitOfWork = unitOfWork;
            Container = container;
            Repositories = new Dictionary<Type, IRepository>();
        }

        public IRepository<TAggergateRoot> GetRepository<TAggergateRoot>()
            where TAggergateRoot : class
        {
            IRepository repository;
            if (!Repositories.TryGetValue(typeof(IRepository<TAggergateRoot>), out repository))
            {
                //repository = Container.Resolve<IRepository<TAggergateRoot>>(new Parameter("dbContext", DbContext),
                //                                                            new Parameter("unitOfWork", UnitOfWork));
                repository = Container.Resolve<IRepository<TAggergateRoot>>();
                Repositories.Add(typeof(IRepository<TAggergateRoot>), repository);
            }

            return repository as IRepository<TAggergateRoot>;
        }

        public void Add<TAggregateRoot>(IEnumerable<TAggregateRoot> entities)
            where TAggregateRoot : class
        {
            GetRepository<TAggregateRoot>().Add(entities);
        }

        public void Add<TaggergateRoot>(TaggergateRoot entity)
            where TaggergateRoot : class
        {
            GetRepository<TaggergateRoot>().Add(entity);
        }

        public TAggergateRoot GetByKey<TAggergateRoot>(params object[] keyValue)
            where TAggergateRoot : class
        {
            return GetRepository<TAggergateRoot>().GetByKey(keyValue);
        }

        public Task<TAggergateRoot> GetByKeyAsync<TAggergateRoot>(params object[] keyValues)
            where TAggergateRoot : class
        {
            return GetRepository<TAggergateRoot>().GetByKeyAsync(keyValues);
        }

        public long Count<TAggergateRoot>(ISpecification<TAggergateRoot> specification)
            where TAggergateRoot : class
        {
            return GetRepository<TAggergateRoot>().Count(specification);
        }

        public Task<long> CountAsync<TAggergateRoot>(ISpecification<TAggergateRoot> specification)
            where TAggergateRoot : class
        {
            return GetRepository<TAggergateRoot>().CountAsync(specification);
        }

        public long Count<TAggergateRoot>(Expression<Func<TAggergateRoot, bool>> specification)
            where TAggergateRoot : class
        {
            return GetRepository<TAggergateRoot>().Count(specification);
        }

        public Task<long> CountAsync<TAggergateRoot>(Expression<Func<TAggergateRoot, bool>> specification)
            where TAggergateRoot : class
        {
            return GetRepository<TAggergateRoot>().CountAsync(specification);
        }

        public void Reload<TEntity>(TEntity entity)
            where TEntity : Entity
        {
            GetRepository<TEntity>().Reload(entity);
        }

        public Task ReloadAsync<TEntity>(TEntity entity)
            where TEntity : Entity
        {
            return GetRepository<TEntity>().ReloadAsync(entity);
        }

        public IQueryable<TAggergateRoot> FindAll<TAggergateRoot>(params OrderExpression[] orderExpressions)
            where TAggergateRoot : class
        {
            return GetRepository<TAggergateRoot>().FindAll(orderExpressions);
        }

        public IQueryable<TAggergateRoot> FindAll<TAggergateRoot>(ISpecification<TAggergateRoot> specification,
            params OrderExpression[] orderExpressions) where TAggergateRoot : class
        {
            return GetRepository<TAggergateRoot>().FindAll(specification, orderExpressions);
        }

        public IQueryable<TAggergateRoot> FindAll<TAggergateRoot>(Expression<Func<TAggergateRoot, bool>> specification,
            params OrderExpression[] orderExpressions) where TAggergateRoot : class
        {
            return GetRepository<TAggergateRoot>().FindAll(specification, orderExpressions);
        }

        public TAggergateRoot Find<TAggergateRoot>(ISpecification<TAggergateRoot> specification)
            where TAggergateRoot : class
        {
            return GetRepository<TAggergateRoot>().Find(specification);
        }

        public Task<TAggergateRoot> FindAsync<TAggergateRoot>(ISpecification<TAggergateRoot> specification)
            where TAggergateRoot : class
        {
            return GetRepository<TAggergateRoot>().FindAsync(specification);
        }

        public TAggergateRoot Find<TAggergateRoot>(Expression<Func<TAggergateRoot, bool>> specification)
            where TAggergateRoot : class
        {
            return GetRepository<TAggergateRoot>().Find(specification);
        }

        public Task<TAggergateRoot> FindAsync<TAggergateRoot>(Expression<Func<TAggergateRoot, bool>> specification)
            where TAggergateRoot : class
        {
            return GetRepository<TAggergateRoot>().FindAsync(specification);
        }

        public bool Exists<TAggregateRoot>(ISpecification<TAggregateRoot> specification)
            where TAggregateRoot : class
        {
            return GetRepository<TAggregateRoot>().Exists(specification);
        }

        public Task<bool> ExistsAsync<TAggregateRoot>(ISpecification<TAggregateRoot> specification)
            where TAggregateRoot : class
        {
            return GetRepository<TAggregateRoot>().ExistsAsync(specification);
        }

        public bool Exists<TAggregateRoot>(Expression<Func<TAggregateRoot, bool>> specification)
            where TAggregateRoot : class
        {
            return GetRepository<TAggregateRoot>().Exists(specification);
        }

        public Task<bool> ExistsAsync<TAggregateRoot>(Expression<Func<TAggregateRoot, bool>> specification)
            where TAggregateRoot : class
        {
            return GetRepository<TAggregateRoot>().ExistsAsync(specification);
        }

        public void Remove<TAggregateRoot>(TAggregateRoot entity) where TAggregateRoot : class
        {
            GetRepository<TAggregateRoot>().Remove(entity);
        }

        public void Remove<TAggregateRoot>(IEnumerable<TAggregateRoot> entities) where TAggregateRoot : class
        {
            GetRepository<TAggregateRoot>().Remove(entities);
        }

        public void Update<TAggregateRoot>(TAggregateRoot entity) where TAggregateRoot : class
        {
            GetRepository<TAggregateRoot>().Update(entity);
        }

        public IQueryable<TAggregateRoot> PageFind<TAggregateRoot>(int pageIndex,
                                                                   int pageSize,
                                                                   Expression<Func<TAggregateRoot, bool>> specification,
                                                                   params OrderExpression[] orderExpressions)
            where TAggregateRoot : class
        {
            return GetRepository<TAggregateRoot>().PageFind(pageIndex, pageSize, specification, orderExpressions);
        }

        public IQueryable<TAggregateRoot> PageFind<TAggregateRoot>(int pageIndex,
                                                                   int pageSize,
                                                                   Expression<Func<TAggregateRoot, bool>> specification,
                                                                   ref long totalCount,
                                                                   params OrderExpression[] orderExpressions)
            where TAggregateRoot : class
        {
            return GetRepository<TAggregateRoot>().PageFind(pageIndex, pageSize, specification, ref totalCount, orderExpressions);
        }

        public Task<Tuple<IQueryable<TAggregateRoot>, long>> PageFindAsync<TAggregateRoot>(int pageIndex,
                                                                                           int pageSize,
                                                                                           Expression<Func<TAggregateRoot, bool>> specification,
                                                                                           params OrderExpression[] orderExpressions)
                                                                                           where TAggregateRoot : class
        {
            return GetRepository<TAggregateRoot>().PageFindAsync(pageIndex, pageSize, specification, orderExpressions);
        }

        public IQueryable<TAggregateRoot> PageFind<TAggregateRoot>(int pageIndex,
                                                                   int pageSize,
                                                                   ISpecification<TAggregateRoot> specification,
                                                                   ref long totalCount,
                                                                   params OrderExpression[] orderExpressions)
            where TAggregateRoot : class
        {
            return GetRepository<TAggregateRoot>().PageFind(pageIndex, pageSize, specification, ref totalCount, orderExpressions);
        }

        public Task<Tuple<IQueryable<TAggregateRoot>, long>> PageFindAsync<TAggregateRoot>(int pageIndex,
                                                                                           int pageSize,
                                                                                           ISpecification<TAggregateRoot> specification,
                                                                                           params OrderExpression[] orderExpressions)
            where TAggregateRoot : class
        {
            return GetRepository<TAggregateRoot>().PageFindAsync(pageIndex, pageSize, specification, orderExpressions);
        }

        public IQueryable<TAggregateRoot> PageFind<TAggregateRoot>(int pageIndex,
                                                                   int pageSize,
                                                                   ISpecification<TAggregateRoot> specification,
                                                                   params OrderExpression[] orderExpressions)
            where TAggregateRoot : class
        {
            return GetRepository<TAggregateRoot>().PageFind(pageIndex, pageSize, specification, orderExpressions);
        }
    }
}