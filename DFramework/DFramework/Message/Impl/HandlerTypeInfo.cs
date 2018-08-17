using System;

namespace DFramework.Message.Impl
{
    public class HandlerTypeInfo
    {
        public HandlerTypeInfo(Type type, bool isAsync)
        {
            Type = type;
            IsAsync = isAsync;
        }

        public Type Type { get; set; }
        public bool IsAsync { get; set; }
    }
}