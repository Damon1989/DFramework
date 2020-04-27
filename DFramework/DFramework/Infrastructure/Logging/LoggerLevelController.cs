using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCommon;

namespace DFramework.Infrastructure.Logging
{
    public class LoggerLevelController : ILoggerLevelController
    {
        protected Level DefaultLevel;

        protected static ConcurrentDictionary<string, ConcurrentDictionary<string, Level>> AppLoggerLevels =
            new ConcurrentDictionary<string, ConcurrentDictionary<string, Level>>();

        protected static ConcurrentDictionary<string, Level> AppDefaultLevels = new ConcurrentDictionary<string, Level>();

        public void SetDefaultLevel(Level defaultLevel)
        {
            DefaultLevel = defaultLevel;
        }

        public Level GetOrAddLoggerLevel(string app, string name, Level? level)
        {
            var appLoggerLevels = AppLoggerLevels.GetOrAdd(app, key => new ConcurrentDictionary<string, Level>());
            return appLoggerLevels.GetOrAdd(name, key => level ?? AppDefaultLevels.TryGetValue(app, DefaultLevel));
        }

        public void SetLoggerLevel(string app, string name, Level level)
        {
            var appLoggerLevels = AppLoggerLevels.GetOrAdd(app, key => new ConcurrentDictionary<string, Level>());
            level = appLoggerLevels.AddOrUpdate(name, key => level, (key, value) => level);
            OnLoggerLevelChanged?.Invoke(app, name, level);
        }

        public void SetAppDefaultLevel(string app, Level level)
        {
            AppDefaultLevels.AddOrUpdate(app, key => level, (key, value) => level);
        }

        public event LoggerLevelChanged OnLoggerLevelChanged;
    }
}