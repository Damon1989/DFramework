using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFramework.MessageQueue
{
    public interface ISlidingDoor
    {
        int MessageCount { get; }

        void AddOffset(long offset);

        void RemoveOffset(long offset);
    }
}