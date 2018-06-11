using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac.Integration.Mvc;
using DFramework.Autofac;
using DFramework.Config;
using DFramework.EntityFramework.Config;
using DFramework.IoC;
using DFramework.IoC.WebAPi;
using DFramework.KendoUI.Domain;
using DFramework.KendoUI.Models;
using DFramework.KendoUI.Repositories.Impl;
using DFramework.Log4Net.Config;

namespace DFramework.KendoUI
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //Configuration.Instance
            //    .UseAutofacContainer()
            //    .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
            //    .RegisterCommonComponents()
            //    .UseLog4Net("KendoUI")
            //    .UseJsonNet();

            //var container = IoCFactory.Instance.CurrentContainer;

            //RegisterTypes(container, Lifetime.Hierarchical);

            //var childContainer = container.CreateChildContainer();
            //RegisterTypes(childContainer, Lifetime.PerRequest);

            //System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = new HierarchicalDependencyResolver(container);
            //DependencyResolver.SetResolver(new AutofacDependencyResolver(childContainer.GetAutofacContainer()));

            var mvcContainer = IoCConfig.GetMvcConfigurationContainer();
            mvcContainer.RegisterType<Node, Node>();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(mvcContainer.GetAutofacContainer()));
        }
    }
}

public class IoCConfig
{
    public static IContainer GetConfigurationContainer()
    {
        return container.Value;
    }

    public static IContainer GetMvcConfigurationContainer()
    {
        return mvcContainer.Value;
    }

    private static Lazy<IContainer> container = new Lazy<IContainer>(() =>
    {
        Configuration.Instance
            .UseAutofacContainer()
        .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
            .RegisterCommonComponents()
            //.UseLog4Net("KendoUI")
            .UseJsonNet();

        var container = IoCFactory.Instance.CurrentContainer;

        RegisterTypes(container, Lifetime.Hierarchical);
        return container;
    });

    private static Lazy<IContainer> mvcContainer = new Lazy<IContainer>(() =>
      {
          var container = GetConfigurationContainer().CreateChildContainer();
          RegisterTypes(container, Lifetime.Hierarchical);
          return container;
      });

    public static void RegisterTypes(IContainer container, Lifetime lifetime)
    {
        Configuration.Instance.RegisterEntityFrameworkComponents(container, lifetime);
        container.RegisterType<KendoDbContext, KendoDbContext>(lifetime);
        container.RegisterType<IKendoUIRepository, KendoUIRepository>(lifetime);
    }
}