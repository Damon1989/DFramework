using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFramework.Config
{
    [ConfigurationCollection(typeof(HandlerElement), AddItemName = "handler",
        CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class HandlerElementCollection : BaseConfigurationElementCollection<HandlerElement>
    {
    }
}