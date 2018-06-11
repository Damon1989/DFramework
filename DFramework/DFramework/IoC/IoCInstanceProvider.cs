using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace DFramework.IoC
{
    public class IoCInstanceProvider : IInstanceProvider
    {
        private readonly IContainer _container;
        private readonly Type _serviceType;

        public IoCInstanceProvider(Type serviceType)
        {
            _serviceType = serviceType;
            _container = IoCFactory.Instance.CurrentContainer;
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return GetInstance(instanceContext, null);
        }

        public object GetInstance(InstanceContext instanceContext, System.ServiceModel.Channels.Message message)
        {
            return _container.Resolve(_serviceType);
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            if (instance is IDisposable)
            {
                ((IDisposable)instance).Dispose();
            }
        }
    }
}