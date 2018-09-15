using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFramework.Config
{
    public class EndpointElement : ConfigurationElement
    {
        [ConfigurationProperty("messageType", IsRequired = true, IsKey = true)]
        public string MessageType
        {
            get => (string)base["messageType"];
            set => base["messageType"] = value;
        }

        [ConfigurationProperty("assembly", IsRequired = true)]
        public string Assembly
        {
            get => (string)base["assembly"];
            set => base["assembly"] = value;
        }

        [ConfigurationProperty("endpoint", IsRequired = true)]
        public string Endpoint
        {
            get => (string)base["endpoint"];
            set => base["endpoint"] = value;
        }

        [ConfigurationProperty("useInternalTransaction", IsRequired = true)]
        public bool UseInternalTransaction
        {
            get => (bool)base["useInternalTransaction"];
            set => base["useInternalTransaction"] = value;
        }
    }
}