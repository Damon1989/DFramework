using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFramework.Bus;
using DFramework.Command;

namespace DFramework.Event
{
    public interface IEventBus : IBus<IEvent>
    {
        void SendCommand(ICommand command);

        void PublishAnyway(params IEvent[] events);

        IEnumerable<ICommand> GetCommands();

        IEnumerable<IEvent> GetEvents();

        IEnumerable<object> GetSagaResults();

        IEnumerable<IEvent> GetToPublishAnywayMessages();

        void FinishSaga(object sagaResult);

        void ClearMessage();
    }
}