using System;
using System.Runtime.Serialization;

namespace Abp
{
    /// <summary>
    ///  Base Exception type for those are thrown by Abp system for ABP specific exceptions.
    /// </summary>
    [Serializable]
    public class AbpException:Exception
    {
        /// <summary>
        ///  Creates a new <see cref="AbpException"/>
        /// </summary>
        public AbpException()
        {

        }

        /// <summary>
        ///  Creates a new <see cref="AbpException"/>
        /// </summary>
        /// <param name="serializationInfo"></param>
        /// <param name="context"></param>
        public AbpException(SerializationInfo serializationInfo, StreamingContext context)
        :base(serializationInfo,context)
        {

        }

        /// <summary>
        ///  Creates a new <see cref="AbpException"/>
        /// </summary>
        /// <param name="message">Exception message</param>
        public AbpException(string message)
            : base(message)
        {

        }

        /// <summary>
        ///  Creates a new <see cref="AbpException"/>
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public AbpException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}