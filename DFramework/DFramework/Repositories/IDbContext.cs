using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFramework.Repositories
{
    public interface IDbContext
    {
        void RemoveEntity<TEntity>(TEntity entity) where TEntity : class;

        void Reload<TEntity>(TEntity entity) where TEntity : class;

        Task ReloadAsync<TEntity>(TEntity entity) where TEntity : class;
    }
}