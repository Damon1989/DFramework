using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MyMvcTest
{
    using MyMvcTest.Controllers;
    using MyMvcTest.Helper;

    using WebApiClient;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            JsonConfigHelper.Init();

            HttpApi.Register<IUserbApi>().ConfigureHttpApiConfig(
                c =>
                    {
                        c.HttpHost = new Uri("http://jsonplaceholder.typicode.com/");
                        c.FormatOptions.DateTimeFormat = DateTimeFormats.ISO8601_WithMillisecond;
                    });
        }
    }
}
