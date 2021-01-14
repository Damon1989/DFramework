namespace FactoryMethodSample
{
    public interface ILoggerFactory
    {
        ILogger CreateLogger();
    }
}