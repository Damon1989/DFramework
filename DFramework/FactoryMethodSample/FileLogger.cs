using System;

namespace FactoryMethodSample
{
    public class FileLogger : ILogger
    {
        public void WriteLog()
        {
            Console.WriteLine("文件日志记录。");
        }
    }
}