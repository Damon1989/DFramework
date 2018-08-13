using System.Linq;
using System.Linq.Expressions;
using DFramework.Infrastructure;

namespace DFramework.Repositories
{
    public static class OrderExpressionUtility
    {
        public static IQueryable<TEntity> MergeOrderExpresion<TEntity>(this IQueryable<TEntity> query,
                                                                        OrderExpression orderExpression,
                                                                        bool hasSorted = false)
        {
            string orderByCmd;
            if (hasSorted)
            {
                orderByCmd = orderExpression.SortOrder == SortOrder.Descending ? "ThenByDescending" : "ThenBy";
            }
            else
            {
                orderByCmd = orderExpression.SortOrder == SortOrder.Descending ? "OrderByDescending" : "OrderBy";
            }

            LambdaExpression le = null;
            if (orderExpression is OrderExpression<TEntity>)
            {
                var member = (orderExpression as OrderExpression<TEntity>).OrderByExpression.Body;
                if (member is UnaryExpression)
                {
                    member = (member as UnaryExpression).Operand;
                }

                le = Utility.GetLambdaExpression(typeof(TEntity), member);
            }
            else if (!string.IsNullOrWhiteSpace(orderExpression.OrderByField))
            {
                le = Utility.GetLambdaExpression(typeof(TEntity), orderExpression.OrderByField);
            }

            var orderByCallExpression =
                Expression.Call(typeof(Queryable),
                    orderByCmd,
                    new[] { typeof(TEntity), le.Body.Type },
                    query.Expression,
                    le);
            return query.Provider.CreateQuery<TEntity>(orderByCallExpression);
        }
    }
}