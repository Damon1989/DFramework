using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FactoryMethodSample
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            ILoggerFactory factory = new FileLoggerFactory();    //可引入配置文件实现
            var logger = factory.CreateLogger();
            logger.WriteLog();
            */

            var factoryString = ConfigurationManager.AppSettings["factory"];
            var factory = (ILoggerFactory)Assembly.Load("FactoryMethodSample").CreateInstance(factoryString);
            var logger = factory.CreateLogger();
            logger.WriteLog();

            Console.Read();

        }
    }
}
