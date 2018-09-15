using System;
using System.Collections.Generic;
using DFramework.Message.Impl;

namespace DFramework.Message
{
    public interface IHandlerProvider
    {
        object GetHandler(Type messageType);

        IList<HandlerTypeInfo> GetHandlerTypes(Type messageType);
    }
}