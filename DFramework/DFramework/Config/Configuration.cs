using DFramework.Infrastructure;
using DFramework.Infrastructure.Caching;
using DFramework.Infrastructure.Caching.Impl;
using DFramework.Infrastructure.Logging;
using DFramework.IoC;

namespace DFramework.Config
{
    public class Configuration
    {
        public static readonly Configuration Instance = new Configuration();

        public Configuration()
        {
        }

        public bool NeedMessageStore { get; protected set; }

        public Configuration RegisterCommonComponents()
        {
            UseNoneLogger();
            UseMemoryCache();
            RegisterExceptionManager<ExceptionManager>();
            return this;
        }

        public Configuration UseNoneLogger()
        {
            IoCFactory.Instance.CurrentContainer.RegisterType<ILoggerLevelController, LoggerLevelController>(Lifetime
                .Singleton);
            IoCFactory.Instance.CurrentContainer.RegisterInstance(typeof(ILoggerFactory), new MockLoggerFactory());
            return this;
        }

        public Configuration RegisterExceptionManager<TExceptionManager>() where TExceptionManager : IExceptionManager
        {
            IoCFactory.Instance
                .CurrentContainer
                .RegisterType<IExceptionManager, TExceptionManager>(Lifetime.Singleton);
            return this;
        }

        public Configuration UseMemoryCache(Lifetime lifetime = Lifetime.Hierarchical)
        {
            IoCFactory.Instance.CurrentContainer.RegisterType<ICacheManager, MemoryCacheManager>(lifetime);
            return this;
        }
    }
}