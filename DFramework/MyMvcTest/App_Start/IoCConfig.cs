using System;
using IFramework.Config;
using IFramework.IoC;

namespace MyMvcTest.App_Start
{
    /// <summary>
    /// Specifies the IoC configuration for the main container.
    /// </summary>
    public class IoCConfig
    {
        #region IoC Container
        private static Lazy<IContainer> container = new Lazy<IContainer>(() =>
        {
            Configuration.Instance.UseAutofacContainer();
            //Configuration.Instance.RegisterAssemblyTypes("Sample.Web", "Sample.Application");
            
            var container = IoCFactory.Instance.CurrentContainer;
            RegisterTypes(container, Lifetime.Hierarchical);
            return container;
        });

        /// <summary>
        /// Gets the configured IoC container.
        /// </summary>
        public static IContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion
        
        #region Mvc Container
        private static Lazy<IContainer> mvcContainer = new Lazy<IContainer>(() =>
        {
            var container = GetConfiguredContainer().CreateChildContainer();
            RegisterTypes(container, Lifetime.PerRequest);
            return container;
        });
        
        /// <summary>
        /// Gets the configured Mvc container.
        /// </summary>
        public static IContainer GetMvcConfiguredContainer()
        {
           return mvcContainer.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the IoC container.</summary>
        /// <param name="container">The IoC container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as IoC allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IContainer container, Lifetime lifetime)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();
            
            // TODO: Register your orm framework here
            // Configuration.Instance.RegisterEntityFrameworkComponents(container, lifetime);

            // TODO: Register your types here
            // container.RegisterType<IProductRepository, ProductRepository>(lifetime);
        }
    }
}
