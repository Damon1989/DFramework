using DFramework.Config;
using DFramework.Infrastructure;
using DFramework.IoC;

namespace DFramework.JsonNet.Config
{
    public static class FrameworkConfigurationExtension
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static Configuration UseJsonNet(this Configuration configuration)
        {
            IoCFactory.Instance.CurrentContainer
                .RegisterInstance(typeof(IJsonConvert),
                new JsonConvertImpl());
            return configuration;
        }
    }
}