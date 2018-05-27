using DFramework.Config;
using System;
using System.Linq;
using System.Reflection;
using Autofac;
using DFramework.IoC;
using Microsoft.Extensions.Configuration;
using System.IO;
using IContainer = Autofac.IContainer;
using Autofac.Configuration;
using DFramework.Autofac;

namespace DFramework.Config
{
    public static class DFrameworkConfigurationExtension
    {
        public static Configuration RegisterAssemblyTypes(this Configuration configuration,
            params string[] assemblyNames)
        {
            if (assemblyNames != null && assemblyNames.Length > 0)
            {
                var assemblies = assemblyNames.Select(name => Assembly.Load(name)).ToArray();
                configuration.RegisterAssemblyTypes(assemblies);
            }
            return configuration;
        }

        public static Configuration RegisterAssemblyTypes(this Configuration configuration,
            params Assembly[] assemblies)
        {
            if (assemblies != null && assemblies.Length > 0)
            {
                var builder = new ContainerBuilder();
                builder.RegisterAssemblyTypes(assemblies);
                builder.Update(IoCFactory.Instance.CurrentContainer.GetAutofacContainer().ComponentRegistry);
            }
            return configuration;
        }

        public static Configuration UseAutofacContainer(this Configuration configuration,
                                                        IContainer container = null,
                                                        string configFile = "autofacfile.xml")
        {
            if (IoCFactory.IsInit())
            {
                return configuration;
            }
            var builder = new ContainerBuilder();
            if (container == null)
            {
                // Add the configuration to the ConfigurationBuilder.
                var config = new ConfigurationBuilder();

                //config.AddJsonFile comes from Microsoft.Extensions.Configuration.Json
                //config.AddXmlFile comes from Microsoft.Extensions.Configuration.Xml
                var fi = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFile));
                if (fi.Exists)
                {
                    if (fi.Extension.Equals(".xml", StringComparison.OrdinalIgnoreCase))
                    {
                        config.AddXmlFile(configFile);
                    }
                    else if (fi.Extension.Equals(".json", StringComparison.OrdinalIgnoreCase))
                    {
                        config.AddJsonFile(configFile);
                    }
                    var module = new ConfigurationModule(config.Build());
                    builder.RegisterModule(module);
                }
                builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies());
                container = builder.Build();
            }
            else
            {
                builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies());
                builder.Update(container);
            }

            IoCFactory.SetContainer(new ObjectContainer(container));

            return configuration;
        }
    }
}