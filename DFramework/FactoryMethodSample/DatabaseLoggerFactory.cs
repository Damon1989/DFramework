﻿namespace FactoryMethodSample
{
    class DatabaseLoggerFactory : ILoggerFactory
    {
        public ILogger CreateLogger()
        {
            //连接数据库，代码省略
            //创建数据库日志记录器对象
            ILogger logger = new DatabaseLogger();
            //初始化数据库日志记录器，代码省略
            return logger;
        }
    }
}