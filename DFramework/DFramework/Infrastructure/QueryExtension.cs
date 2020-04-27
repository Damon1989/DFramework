using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DCommon;
using DFramework.Repositories;
using DFramework.Specifications;

namespace DFramework.Infrastructure
{
    public static class QueryExtension
    {
        public static IQueryable<TEntity> FindAll<TEntity>(this IQueryable<TEntity> query,
            ISpecification<TEntity> specification,
            params OrderExpression[] orderExpressions)
        {
            return query.FindAll(specification.GetExpression(), orderExpressions);
        }

        public static IQueryable<TEntity> FindAll<TEntity>(this IQueryable<TEntity> query,
            Expression<Func<TEntity, bool>> expression,
            params OrderExpression[] orderExpressions)
        {
            query = query.Where(expression);
            var hasSorted = false;
            orderExpressions.ForEach(orderExpression =>
            {
                query = query.MergeOrderExpresion(orderExpression, hasSorted);
                hasSorted = true;
            });
            return query;
        }
    }
}