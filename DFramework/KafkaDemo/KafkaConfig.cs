using System.Configuration;

namespace KafkaDemo
{
    /// <summary>
    /// kafka配置类
    /// </summary>
    public class KafkaConfig : ConfigurationSection
    {
        /// <summary>
        /// 当前配置名称
        ///  此属性为必须
        /// </summary>
        public string SectionName { get; set; }

        /// <summary>
        ///     代理
        /// </summary>
        [ConfigurationProperty("broker", IsRequired = true)]
        public string Broker
        {
            get => (string)base["broker"];
            set => base["broker"] = value;
        }

        /// <summary>
        ///     主题
        /// </summary>
        [ConfigurationProperty("topic", IsRequired = true)]
        public string Topic
        {
            get => (string)base["topic"];
            set => base["topic"] = value;
        }

        /// <summary>
        /// 获取默认kafka配置类
        /// </summary>
        /// <returns></returns>
        public static KafkaConfig GetConfig()
        {
            return (KafkaConfig)ConfigurationManager.GetSection("kafkaConfig");
        }

        /// <summary>
        /// 获取指定的kafka配置类
        /// </summary>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public static KafkaConfig GetConfig(string sectionName)
        {
            var section = (KafkaConfig)ConfigurationManager.GetSection(sectionName);

            if (section == null) section = GetConfig();

            if (section == null) throw new ConfigurationErrorsException($"kafkaconfig节点{sectionName}未配置");

            section.SectionName = sectionName;
            return section;
        }

        /// <summary>
        ///     从指定位置读取配置
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public static KafkaConfig GetConfig(string fileName, string sectionName)
        {
            return GetConfig(ConfigurationManager.OpenMappedMachineConfiguration(new ConfigurationFileMap(fileName)), sectionName);
        }

        /// <summary>
        ///     从指定Configuration中读取配置
        /// </summary>
        /// <param name="config"></param>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public static KafkaConfig GetConfig(Configuration config, string sectionName)
        {
            if (config == null)
                throw new ConfigurationErrorsException("传入的配置不能为空");
            var section = (KafkaConfig)config.GetSection(sectionName);
            if (section == null)
                throw new ConfigurationErrorsException("kafkacofng节点 " + sectionName + " 未配置.");
            section.SectionName = sectionName;
            return section;
        }
    }
}