using System;

namespace FactoryMethodSample
{
    public class DatabaseLogger:ILogger
    {
        public void WriteLog()
        {
            Console.WriteLine("数据库日志记录。");
        }
    }
}