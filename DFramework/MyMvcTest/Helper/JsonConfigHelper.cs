using System;

namespace MyMvcTest.Helper
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    using Newtonsoft.Json;

    public class JsonConfigHelper
    {
        public static ConfigJson ConfigJson { get; set; }
        private static FileSystemWatcher Watcher { get; set; }
        private static string ConfigFile { get; set; }
        public static void Init(string configFile=null)
        {
            ConfigFile = configFile ?? "jsonConfig.json";
            var file=new FileInfo(ConfigFile);
            if (!file.Exists)
            {
                file=new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigFile));
            }

            if (file.Exists)
            {
                var content = File.ReadAllText(file.FullName);
                ConfigJson = JsonConvert.DeserializeObject<ConfigJson>(content);

                Watcher = new FileSystemWatcher
                              {
                                  Path = AppDomain.CurrentDomain.BaseDirectory,
                                  Filter = ConfigFile,
                                  NotifyFilter = NotifyFilters.LastWrite,
                                  IncludeSubdirectories = false,
                                  EnableRaisingEvents = true
                              };
                Watcher.Changed += Watcher_Changed;
            }
        }

        private static void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            var file = new FileInfo(ConfigFile);
            if (!file.Exists)
            {
                file = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigFile));
            }

            if (file.Exists)
            {
                var fs=new FileStream(file.FullName,FileMode.Open,FileAccess.Read,FileShare.ReadWrite);
                int len = (int)fs.Length;
                var byteInfo=new byte[len];
                fs.Read(byteInfo, 0, len);
                var content= Encoding.UTF8.GetString(byteInfo, 0, len);
                ConfigJson = JsonConvert.DeserializeObject<ConfigJson>(content);
            }
        }
    }

    public class ConfigJson
    {
        public bool UseLog { get; set; }

        public List<string> District { get; set; }

    }
}