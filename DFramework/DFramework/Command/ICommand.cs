using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFramework.Message;

namespace DFramework.Command
{
    public interface ICommand : IMessage
    {
        bool NeedRetry { get; set; }
    }

    public interface ILinearCommand : ICommand { }
}