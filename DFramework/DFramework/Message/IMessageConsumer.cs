using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFramework.Message
{
    public interface IMessageConsumer
    {
        decimal MessageCount { get; }

        void Start();

        void Stop();

        string GetStatus();
    }
}