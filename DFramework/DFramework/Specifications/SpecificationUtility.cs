using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DFramework.Specifications
{
    public class ParameterRebinder : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> Map;

        public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            this.Map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map,
            Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            ParameterExpression replacement;
            if (Map.TryGetValue(node, out replacement))
            {
                node = replacement;
            }
            return base.VisitParameter(node);
        }
    }

    public static class ExpressionUtility
    {
        public static Expression<T> Compose<T>(this Expression<T> first,
                                               Expression<T> second,
                                               Func<Expression, Expression, Expression> merge)
        {
            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] })
                .ToDictionary(p => p.s, p => p.f);
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.Add);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.Or);
        }
    }
}