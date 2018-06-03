using DFramework.Message;

namespace DFramework.Command
{
    public interface ICommand : IMessage
    {
        bool NeedRetry { get; set; }
    }

    public interface ILinearCommand : ICommand { }
}