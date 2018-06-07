using System;

namespace DFramework.Infrastructure.Logging
{
    /// <summary>
    /// Represents a logger factory.
    /// </summary>
    public interface ILoggerFactory
    {
        /// <summary>
        /// Create a logger with the given logger name,app,module.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="app"></param>
        /// <param name="module"></param>
        /// <param name="level"></param>
        /// <param name="additionalProperties"></param>
        /// <returns></returns>
        ILogger Create(string name, string app = null, string module = null, Level? level = null, object additionalProperties = null);

        /// <summary>
        /// Create a logger with the given type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="level"></param>
        /// <param name="addicationalProperties"></param>
        /// <returns></returns>
        ILogger Create(Type type, Level? level = null, object addicationalProperties = null);
    }
}