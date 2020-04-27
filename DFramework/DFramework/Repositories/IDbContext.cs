namespace DFramework.Repositories
{
    using System.Threading.Tasks;

    public interface IDbContext
    {
        void Reload<TEntity>(TEntity entity)
            where TEntity : class;

        Task ReloadAsync<TEntity>(TEntity entity)
            where TEntity : class;

        void RemoveEntity<TEntity>(TEntity entity)
            where TEntity : class;
    }
}