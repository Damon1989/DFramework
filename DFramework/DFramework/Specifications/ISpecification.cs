using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DFramework.Specifications
{
    public interface ISpecification<T>
    {
        bool IsSatisfiedBy(T obj);

        ISpecification<T> Add(ISpecification<T> other);

        ISpecification<T> Or(ISpecification<T> other);

        ISpecification<T> AndNot(ISpecification<T> other);

        ISpecification<T> Not();

        Expression<Func<T, bool>> GetExpression();
    }
}