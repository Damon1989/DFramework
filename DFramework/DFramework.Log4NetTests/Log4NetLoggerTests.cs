using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFramework.Config;
using DFramework.Infrastructure.Logging;
using DFramework.IoC;
using DFramework.Log4Net.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static System.Console;

namespace DFramework.Log4NetTests
{
    [TestClass]
    //[DeploymentItem("log4net.config", "")]
    public class Log4NetLoggerTests
    {
        [TestInitialize]
        public void Initialize()
        {
            Configuration.Instance
                .UseAutofacContainer()
                .RegisterCommonComponents()
                .UseLog4Net("Log4NetLoggerTest");
        }

        [TestMethod]
        public void TestLog()
        {
            var loggerFactory = IoCFactory.Resolve<ILoggerFactory>();
            var logger = loggerFactory.Create(nameof(Log4NetLoggerTests));
            var message = "test log level";
            WriteLine(logger.Level);
            LogTest(logger, message);

            logger.ChangeLogLevel(Level.Debug);
            Console.WriteLine(logger.Level);
            LogTest(logger, message);

            logger.ChangeLogLevel(Level.Info);
            Console.WriteLine(logger.Level);
            LogTest(logger, message);

            logger.ChangeLogLevel(Level.Warn);
            Console.WriteLine(logger.Level);
            LogTest(logger, message);

            logger.ChangeLogLevel(Level.Error);
            Console.WriteLine(logger.Level);
            LogTest(logger, message);

            logger.ChangeLogLevel(Level.Fatal);
            Console.WriteLine(logger.Level);
            LogTest(logger, message);
        }

        private void LogTest(ILogger logger, string message)
        {
            logger.Debug(message);
            logger.Info(message);
            logger.Warn(message);
            logger.Error(message);
            logger.Fatal(message);
        }
    }
}