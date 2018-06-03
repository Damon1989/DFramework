using System;
using System.Linq.Expressions;

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