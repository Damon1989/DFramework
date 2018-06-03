using System;
using System.Linq.Expressions;

namespace DFramework.Specifications
{
    public sealed class NoneSpecification<T> : Specification<T>
    {
        public override Expression<Func<T, bool>> GetExpression()
        {
            return o => false;
        }
    }
}