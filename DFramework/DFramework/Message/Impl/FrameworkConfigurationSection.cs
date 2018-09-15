using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFramework.Config;

namespace DFramework.Message.Impl
{
    [ConfigurationSectionName("frameworkConfiguration")]
    public class FrameworkConfigurationSection : ConfigurationSection
    {
        /// <summary>
        ///     Gets or sets the configuration settings for handlers.
        /// </summary>
        [ConfigurationProperty("handlers", IsRequired = false)]
        public HandlerElementCollection Handlers
        {
            get => (HandlerElementCollection)base["handlers"];
            set => base["handlers"] = value;
        }

        [ConfigurationProperty("messageEndpointMappings", IsRequired = false)]
        public MessageEndpointElementCollection MessageEndpointMappings
        {
            get => (MessageEndpointElementCollection)base["messageEndpointMappings"];
            set => base["messageEndpointMappings"] = value;
        }
    }
}