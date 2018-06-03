using System;
using System.Linq.Expressions;

namespace DFramework.Specifications
{
    [Semantics(Semantics.All)]
    public sealed class AllSpecification<T> : Specification<T>
    {
        public override Expression<Func<T, bool>> GetExpression()
        {
            return e => true;
        }
    }
}