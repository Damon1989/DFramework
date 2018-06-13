using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;
using DFramework.Event;
using DFramework.Infrastructure;

namespace DFramework.Exceptions
{
    public class ErrorCodeDictionary
    {
        private static readonly Dictionary<object, string> ErrorCodeDic = new Dictionary<object, string>();

        public static string GetErrorMessage(object errorCode, params object[] args)
        {
            var errorMessage = ErrorCodeDic.TryGetValue(errorCode, string.Empty);
            if (string.IsNullOrEmpty(errorMessage))
            {
                var errorCodeFieldInfo = errorCode.GetType().GetField(errorCode.ToString());
                if (errorCodeFieldInfo != null)
                {
                    errorMessage = errorCodeFieldInfo.GetCustomAttribute<DescriptionAttribute>()?.Description;
                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        errorMessage = errorCode.ToString();
                    }
                }
            }
            if (args != null & args.Length > 0)
            {
                return string.Format(errorMessage, args);
            }
            return errorMessage;
        }

        public static void AddErrorCodeMessage(IDictionary<object, string> dictionary)
        {
            dictionary.ForEach(p =>
            {
                if (ErrorCodeDic.ContainsKey(p.Key))
                {
                    throw new Exception($"ErrorCode dictionary has already had the key {p.Key}");
                }
                ErrorCodeDic.Add(p.Key, p.Value);
            });
        }
    }

    public class DomainException : Exception
    {
        public IDomainExceptionEvent DomainExceptionEvent { get; protected set; }

        public int ErrorCode { get; set; }

        public DomainException()
        {
        }

        public DomainException(IDomainExceptionEvent domainExceptionEvent)
            : this(Exceptions.ErrorCode.UnknownError, domainExceptionEvent.ToString())
        {
            DomainExceptionEvent = domainExceptionEvent;
        }

        protected DomainException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            ErrorCode = (int)info.GetValue("ErrorCode", typeof(int));
        }

        public DomainException(object errorCode, string message = null)
            : base(message ?? ErrorCodeDictionary.GetErrorMessage(errorCode))
        {
            ErrorCode = (int)errorCode;
        }

        public DomainException(object errorCode, object[] args)
            : base(ErrorCodeDictionary.GetErrorMessage(errorCode, args))
        {
            ErrorCode = (int)errorCode;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ErrorCode", ErrorCode);
            info.AddValue("DomainExceptionEvent", DomainExceptionEvent);
            base.GetObjectData(info, context);
        }
    }
}