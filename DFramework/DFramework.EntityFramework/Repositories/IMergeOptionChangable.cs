using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFramework.EntityFramework.Repositories
{
    public interface IMergeOptionChangable
    {
        void ChangeMergeOption<TEntity>(MergeOption mergeOption) where TEntity : class;
    }
}