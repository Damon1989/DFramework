using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace MyMvcTest.Helper
{
    public static class WebConfigHelper
    {
        public static Dictionary<string, string> GetWebConfigSettings()
        {
            var appSetting = (AppSettingsSection) GetConfigurationSection("appSettings");

            return appSetting.Settings.AllKeys.ToDictionary(key => key, key => appSetting.Settings[key].Value);
        }

        private static Configuration GetConfiguration()
        {
            var path = HttpContext.Current.Request.ApplicationPath;
            return WebConfigurationManager.OpenWebConfiguration(path);
        }

        private static ConfigurationSection GetConfigurationSection(string sectionName)
        {
            var config = GetConfiguration();
            return config.GetSection(sectionName);
        }

        public static void ModifyAppSetting(string key, string value)
        {
            var config = GetConfiguration();
            var appSetting = (AppSettingsSection) config.GetSection("appSettings");
            if (appSetting.Settings[key] == null)
                appSetting.Settings.Add(key, value);
            else
                appSetting.Settings[key].Value = value;
            config.Save();
            config = null;
        }
    }
}