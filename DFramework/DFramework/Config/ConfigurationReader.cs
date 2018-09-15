using System.Collections;
using System.Configuration;
using DFramework.Infrastructure;

namespace DFramework.Config
{
    public sealed class ConfigurationReader
    {
        private readonly Hashtable Configs = new Hashtable();

        /// <summary>
        ///     Initializes a new instance of <c>Configuration Reader</c> class.
        /// </summary>
        private ConfigurationReader()
        {
        }

        public static ConfigurationReader Instance { get; } = new ConfigurationReader();

        public TConfigurationSection GetConfigurationSection<TConfigurationSection>(string name = null)
            where TConfigurationSection : ConfigurationSection
        {
            if (string.IsNullOrEmpty(name))
            {
                var configSectionNameAttr = typeof(TConfigurationSection)
                    .GetCustomAttribute<ConfigurationSectionNameAttribute>();
                if (configSectionNameAttr != null)
                    name = configSectionNameAttr.Name;
                if (string.IsNullOrEmpty(name))
                    name = typeof(TConfigurationSection).Name;
            }

            var configSection = Configs[name] as TConfigurationSection;
            if (configSection == null)
            {
                configSection = ConfigurationManager.GetSection(name) as TConfigurationSection;
                Configs[name] = configSection;
            }
            return configSection;
        }
    }
}