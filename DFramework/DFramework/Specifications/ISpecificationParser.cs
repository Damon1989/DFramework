using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFramework.Specifications
{
    public interface ISpecificationParser<T>
    {
        T Parse<TEntity>(ISpecification<TEntity> specification);
    }
}