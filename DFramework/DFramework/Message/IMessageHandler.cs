﻿namespace DFramework.Message
{
    public interface IMessageHandler
    {
        void Handle(object message);
    }

    public interface IMessageHandler<in TMessage> where TMessage : class
    {
        void Handler(TMessage message);
    }
}