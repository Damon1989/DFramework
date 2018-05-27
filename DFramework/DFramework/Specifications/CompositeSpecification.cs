using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFramework.Specifications
{
    public abstract class CompositeSpecification<T> : Specification<T>, ICompositeSpecification<T>
    {
        public CompositeSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            Left = left;
            Right = right;
        }

        public ISpecification<T> Left { get; }
        public ISpecification<T> Right { get; }
    }
}