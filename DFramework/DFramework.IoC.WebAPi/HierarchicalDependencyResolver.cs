using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;

namespace DFramework.IoC.WebAPi
{
    public class HierarchicalDependencyResolver : IDependencyResolver
    {
        private readonly IContainer container;

        public HierarchicalDependencyResolver(IContainer container)
        {
            if (container == null)
            {
                throw new ArgumentException("container");
            }

            this.container = container.CreateChildContainer();
        }

        public void Dispose()
        {
            container.Dispose();
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return container.Resolve(serviceType);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return container.ResolveAll(serviceType);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public IDependencyScope BeginScope()
        {
            return new AutofacHierarchicalDependencyScope(container);
        }

        private sealed class AutofacHierarchicalDependencyScope : IDependencyScope
        {
            private readonly IContainer container;

            public AutofacHierarchicalDependencyScope(IContainer parentContainer)
            {
                container = parentContainer.CreateChildContainer();
            }

            public void Dispose()
            {
                container.Dispose();
            }

            public object GetService(Type serviceType)
            {
                return container.Resolve(serviceType);
            }

            public IEnumerable<object> GetServices(Type serviceType)
            {
                return container.ResolveAll(serviceType);
            }
        }
    }
}