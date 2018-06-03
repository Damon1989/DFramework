using System;
using System.Linq.Expressions;

namespace DFramework.Specifications
{
    public abstract class Specification<T> : ISpecification<T>
    {
        public static Specification<T> Eval(Expression<Func<T, bool>> expression)
        {
            return new ExpressionSpecification<T>(expression);
        }

        public bool IsSatisfiedBy(T obj)
        {
            return GetExpression().Compile()(obj);
        }

        public ISpecification<T> Add(ISpecification<T> other)
        {
            return new AndSpecification<T>(this, other);
        }

        public ISpecification<T> Or(ISpecification<T> other)
        {
            return new OrSpecification<T>(this, other);
        }

        public ISpecification<T> AndNot(ISpecification<T> other)
        {
            return new AndNotSpecification<T>(this, other);
        }

        public ISpecification<T> Not()
        {
            return new NotSpecification<T>(this);
        }

        public abstract Expression<Func<T, bool>> GetExpression();
    }
}