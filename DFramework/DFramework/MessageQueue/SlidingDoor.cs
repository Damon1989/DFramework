using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFramework.MessageQueue
{
    public class SlidingDoor : ISlidingDoor
    {
        private readonly object _removeOffsetLock = new object();
        protected string _broker;
        protected Action<string, int, long> _commitOffset;
        protected bool _commitPerMessage;
        protected long _consumedOffset = -1L;
        protected long _lastCommittedOffset = -1L;
        protected long _lastOffset = -1L;
        protected SortedSet<long> _offsets;
        protected int _partition;

        public SlidingDoor(Action<string, int, long> commitOffset,
            string broker,
            int partition,
            bool commitPerMessage = false)
        {
            _commitOffset = commitOffset;
            _broker = broker;
            _partition = partition;
            _offsets = new SortedSet<long>();
            _commitPerMessage = commitPerMessage;
        }

        public int MessageCount => _offsets.Count;

        public void AddOffset(long offset)
        {
            lock (_removeOffsetLock)
            {
                _offsets.Add(offset);
                _lastOffset = offset;
            }
        }

        public void RemoveOffset(long offset)
        {
            if (_commitPerMessage)
            {
                _commitOffset(_broker, _partition, offset);
                lock (_removeOffsetLock)
                {
                    _offsets.Remove(offset);
                }
            }
            else
            {
                lock (_removeOffsetLock)
                {
                    if (_offsets.Remove(offset))
                    {
                        if (_offsets.Count > 0)
                        {
                            _consumedOffset = _offsets.First() - 1;
                        }
                        else
                        {
                            _consumedOffset = _lastOffset;
                        }
                    }

                    if (_consumedOffset > _lastCommittedOffset)
                    {
                        _commitOffset(_broker, _partition, _consumedOffset);
                        _lastCommittedOffset = _consumedOffset;
                    }
                }
            }
        }
    }
}