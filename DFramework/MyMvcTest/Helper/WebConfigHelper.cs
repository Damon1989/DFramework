using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Configuration;

namespace MyMvcTest.Helper
{
    public static class WebConfigHelper
    {
        public static Dictionary<string, string> GetWebConfigSettings()
        {
            var result = new Dictionary<string, string>();
            var appSetting = (AppSettingsSection) GetConfigurationSection("appSettings");

            foreach (var key in appSetting.Settings.AllKeys) result.Add(key, appSetting.Settings[key].Value);

            return result;
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