using DFramework.Event;
using DFramework.Infrastructure.Logging;
using DFramework.UnitOfWork;

namespace DFramework.EntityFramework
{
    public class AppUnitOfWork : UnitOfWork, IAppUnitOfWork
    {
        //public AppUnitOfWork(IEventBus eventBus, ILoggerFactory loggerFactory) : base(eventBus, loggerFactory)
        //{
        //}
        public AppUnitOfWork(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }
    }
}