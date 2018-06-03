using System;
using System.Linq.Expressions;

namespace DFramework.Specifications
{
    [Semantics(Semantics.And)]
    public class AndSpecification<T> : CompositeSpecification<T>
    {
        public AndSpecification(ISpecification<T> left, ISpecification<T> right)
            : base(left, right)
        {
        }

        public override Expression<Func<T, bool>> GetExpression()
        {
            var body = Left.GetExpression().And(Right.GetExpression());
            return body;
        }
    }
}