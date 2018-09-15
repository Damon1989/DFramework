using System;
using System.Collections.Generic;
using DFramework.Message.Impl;

namespace DFramework.Message
{
    public interface IMessageStore : IDisposable
    {
        CommandHandledInfo GetCommandHandledInfo(string commandId);

        bool HasEventHandled(string eventId, string subscriptionName);

        void HandleEvent(IMessageContext eventContext, string subscriptionName,
                                     IEnumerable<IMessageContext> commandContexts,
                                     IEnumerable<IMessageContext> messageContexts);

        void SaveEvent(IMessageContext eventContext);

        void SaveFailHandledEvent(IMessageContext eventContext, string subscriotionName, Exception e,
                                                    params IMessageContext[] messageContexts);

        void SaveCommand(IMessageContext commandContext, object result, params IMessageContext[] eventContexts);

        void SaveFiledCommand(IMessageContext commandContext, Exception e = null,
                                                params IMessageContext[] eventContexts);

        void RemoveSentCommand(string commandId);

        void RemovePublishedEvent(string eventId);

        IEnumerable<IMessageContext> GetAllUnSentCommands(
            Func<string, IMessage, string, string, string, SagaInfo, string, IMessageContext> warpMessage);

        IEnumerable<IMessageContext> GetAllUnPublishedEvents(
            Func<string, IMessage, string, string, string, SagaInfo, string, IMessageContext> warpMessage);

        void Rollback();
    }
}