using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DFramework.Specifications
{
    public class NotSpecification<T> : Specification<T>
    {
        private readonly ISpecification<T> spec;

        public NotSpecification(ISpecification<T> specification)
        {
            spec = specification;
        }

        public override Expression<Func<T, bool>> GetExpression()
        {
            var body = Expression.Not(spec.GetExpression().Body);
            return Expression.Lambda<Func<T, bool>>(body, spec.GetExpression().Parameters);
        }
    }
}