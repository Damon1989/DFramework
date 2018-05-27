using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFramework.Infrastructure.Logging
{
    public class JsonLogBase
    {
        public string Host { get; set; }
        public string Ip { get; set; }
        public string App { get; set; }
        public string Module { get; set; }
        public string UserName { get; set; }
        public string Logger { get; set; }
        public string LogLevel { get; set; }
        public string Time { get; set; }
        public string Method { get; set; }
        public string Thread { get; set; }
        public string Target { get; set; }
        public object Message { get; set; }
        public LogException Exception { get; set; }
    }

    public class LogException
    {
        public string Class { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
    }
}