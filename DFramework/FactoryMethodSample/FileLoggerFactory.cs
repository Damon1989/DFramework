namespace FactoryMethodSample
{
    class FileLoggerFactory : ILoggerFactory
    {
        public ILogger CreateLogger()
        {
            //创建文件日志记录器对象
            ILogger logger = new FileLogger();
            //创建文件，代码省略
            return logger;
        }
    }
}