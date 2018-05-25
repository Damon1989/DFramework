using DFramework.Infrastructure.Caching;
using DFramework.Infrastructure.Caching.Impl;
using DFramework.IoC;

namespace DFramework.Config
{
    public class Configuration
    {
        public static readonly Configuration Instance = new Configuration();

        public Configuration()
        {
        }

        public Configuration UseMemoryCache(Lifetime lifetime = Lifetime.Hierarchical)
        {
            IoCFactory.Instance.CurrentContainer.RegisterType<ICacheManager, MemoryCacheManager>(lifetime);
            return this;
        }
    }
}