using System.Data.Entity.Core.Objects;
using DFramework.IoC;
using DFramework.UnitOfWork;

namespace DFramework.EntityFramework.Repositories
{
    public class DomainRepository : DFramework.Repositories.DomainRepository, IMergeOptionChangable
    {
        public DomainRepository(object dbContext, IUnitOfWork unitOfWork, IContainer container)
            : base(dbContext, unitOfWork, container)
        {
        }

        public void ChangeMergeOption<TEntity>(MergeOption mergeOption)
            where TEntity : class
        {
            if (GetRepository<TEntity>() is IMergeOptionChangable repository)
            {
                repository.ChangeMergeOption<TEntity>(mergeOption);
            }
        }
    }
}