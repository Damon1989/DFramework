﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DFramework.Specifications;

namespace DFramework.Repositories
{
    public interface IRepository
    {
    }

    public interface IRepository<TAggergateRoot> : IRepository
        where TAggergateRoot : class
    {
        void Add(IEnumerable<TAggergateRoot> entities);

        void Add(TAggergateRoot entity);

        TAggergateRoot GetByKey(params object[] keyValues);

        Task<TAggergateRoot> GetByKeyAsync(params object[] keyValues);

        long Count(ISpecification<TAggergateRoot> specification);

        Task<long> CountAsync(ISpecification<TAggergateRoot> specification);

        long Count(Expression<Func<TAggergateRoot, bool>> specification);

        Task<long> CountAsync(Expression<Func<TAggergateRoot, bool>> specification);

        IQueryable<TAggergateRoot> FindAll(params OrderExpression[] orderExpressions);

        IQueryable<TAggergateRoot> FindAll(ISpecification<TAggergateRoot> specification,
                                            params OrderExpression[] orderExpressions);

        IQueryable<TAggergateRoot> FindAll(Expression<Func<TAggergateRoot, bool>> specification,
                                            params OrderExpression[] orderExpressions);

        TAggergateRoot Find(ISpecification<TAggergateRoot> specification);

        Task<TAggergateRoot> FindAsync(ISpecification<TAggergateRoot> specification);

        TAggergateRoot Find(Expression<Func<TAggergateRoot, bool>> specification);

        Task<TAggergateRoot> FindAsync(Expression<Func<TAggergateRoot, bool>> specification);

        bool Exists(ISpecification<TAggergateRoot> specification);

        Task<bool> ExistsAsync(ISpecification<TAggergateRoot> specification);

        bool Exists(Expression<Func<TAggergateRoot, bool>> specification);

        Task<bool> ExistsAsync(Expression<Func<TAggergateRoot, bool>> specification);

        void Remove(TAggergateRoot entity);

        void Remove(IEnumerable<TAggergateRoot> entities);

        void Reload(TAggergateRoot entity);

        Task ReloadAsync(TAggergateRoot entity);

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