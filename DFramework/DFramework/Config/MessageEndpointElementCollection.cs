using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFramework.Config
{
    [ConfigurationCollection(typeof(EndpointElement), AddItemName = "endpoint",
        CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class MessageEndpointElementCollection : BaseConfigurationElementCollection<EndpointElement>
    {
    }
}