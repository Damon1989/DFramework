namespace DFramework.Infrastructure.Logging
{
    public delegate void LoggerLevelChanged(string app, string logger, Level level);

    public interface ILoggerLevelController
    {
        void SetDefaultLevel(Level defaultLevel);

        Level GetOrAddLoggerLevel(string app, string name, Level? level);

        void SetLoggerLevel(string app, string name, Level level);

        void SetAppDefaultLevel(string app, Level level);

        event LoggerLevelChanged OnLoggerLevelChanged;
    }
}