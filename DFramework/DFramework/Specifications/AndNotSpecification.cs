using System;
using System.Linq.Expressions;

namespace DFramework.Specifications
{
    [Semantics(Semantics.AndNot)]
    public class AndNotSpecification<T> : CompositeSpecification<T>
    {
        public AndNotSpecification(ISpecification<T> left, ISpecification<T> right)
            : base(left, right)
        {
        }

        public override Expression<Func<T, bool>> GetExpression()
        {
            var bodyNot = Expression.Lambda<Func<T, Boolean>>(Expression.Not(Right.GetExpression().Body));
            var body = Left.GetExpression().And(bodyNot);
            return body;
        }
    }
}