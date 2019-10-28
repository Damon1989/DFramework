using System.Configuration;
using StackExchange.Redis;

namespace ConsoleApp
{
    public class RedisManager
    {
        // Fields
        private static ConnectionMultiplexer _instance;

        private static readonly object Locker = new object();

        // Methods
        private RedisManager()
        {
        }

        // Properties
        public static ConnectionMultiplexer Instance
        {
            get
            {
                if (_instance == null)
                {
                    var locker = Locker;
                    lock (locker)
                    {
                        if (_instance == null)
                            _instance = ConnectionMultiplexer.Connect(ConfigurationManager.AppSettings["RedisHost"],
                                null);
                    }
                }

                return _instance;
            }
        }

        public static IServer Server => Instance.GetServer(ConfigurationManager.AppSettings["RedisHost"]);
    }
}