using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFramework.Infrastructure.Logging
{
    internal class MockLoggerFactory : ILoggerFactory
    {
        public ILogger Create(string name, string app = null, string module = null, Level? level = null,
            object additionalProperties = null)
        {
            return MockLogger.Instance;
        }

        public ILogger Create(Type type, Level? level = null, object addicationalProperties = null)
        {
            return MockLogger.Instance;
        }
    }
}