using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using DFramework.Infrastructure;

namespace DFramework.AspNet.IPRestriction
{
    public class IPRestirctConfig
    {
        public IPRestirctConfig()
        {
            GlobalWhiteList = new List<string>();
            EntryWhiteListDictionary = new Dictionary<string, List<string>>();
        }

        public List<string> GlobalWhiteList { get; set; }
        public Dictionary<string, List<string>> EntryWhiteListDictionary { get; set; }
    }

    public static class IPRestrictExtension
    {
        internal static bool Enabled;
        internal static IPRestirctConfig IPRestrictConfig = new IPRestirctConfig();

        public static HttpConfiguration EnableIPRestrict(this HttpConfiguration config, string configFile = null)
        {
            configFile = configFile ?? "ipRestrict.json";
            var file = new FileInfo(configFile);
            if (!file.Exists)
            {
                file = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFile));
            }

            if (file.Exists)
            {
                var json = File.ReadAllText(file.FullName);
                IPRestrictConfig = json.ToJsonObject<IPRestirctConfig>();
            }

            Enabled = true;
            return config;
        }
    }
}