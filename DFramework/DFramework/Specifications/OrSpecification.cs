using System;
using System.Linq.Expressions;

namespace DFramework.Specifications
{
    public class OrSpecification<T> : CompositeSpecification<T>
    {
        public OrSpecification(ISpecification<T> left, ISpecification<T> right) : base(left, right)
        {
        }

        public override Expression<Func<T, bool>> GetExpression()
        {
            var body = Left.GetExpression().Or(Right.GetExpression());
            return body;
        }
    }
}