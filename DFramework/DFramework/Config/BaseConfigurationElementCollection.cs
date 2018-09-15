using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFramework.Infrastructure;

namespace DFramework.Config
{
    public class BaseConfigurationElementCollection<TConfigurationElement> : ConfigurationElementCollection
        where TConfigurationElement : ConfigurationElement, new()
    {
        public BaseConfigurationElementCollection()
        {
            foreach (var p in typeof(TConfigurationElement).GetProperties())
            {
                var configurationProperty = p.GetCustomAttribute<ConfigurationPropertyAttribute>();
                if (configurationProperty != null && configurationProperty.IsKey)
                {
                    ConfigurationElementKey = p.Name;
                    break;
                }
            }
        }

        private string ConfigurationElementKey { get; }

        public TConfigurationElement this[int idx]
        {
            get => BaseGet(idx) as TConfigurationElement;
            set
            {
                if (BaseGet(idx) != null)
                    BaseRemoveAt(idx);
                BaseAdd(idx, value);
            }
        }

        public override ConfigurationElementCollectionType CollectionType =>
            ConfigurationElementCollectionType.BasicMap;

        protected override string ElementName => this.GetCustomAttribute<ConfigurationCollectionAttribute>()
            .AddItemName;

        protected override ConfigurationElement CreateNewElement()
        {
            return new TConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return element.GetValueByKey(ConfigurationElementKey);
        }
    }
}