using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFramework.Message
{
    public interface IHandlerProvider
    {
        object GetHandler(Type messageType);
    }
}