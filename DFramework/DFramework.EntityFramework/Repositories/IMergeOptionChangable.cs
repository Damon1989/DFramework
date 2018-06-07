using System.Data.Entity.Core.Objects;

namespace DFramework.EntityFramework.Repositories
{
    public interface IMergeOptionChangable
    {
        void ChangeMergeOption<TEntity>(MergeOption mergeOption) where TEntity : class;
    }
}