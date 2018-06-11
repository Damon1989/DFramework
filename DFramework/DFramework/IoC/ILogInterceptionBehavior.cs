using System;
using System.Reflection;
using DFramework.Infrastructure.Logging;

namespace DFramework.IoC
{
    public interface ILogInterceptionBehavior
    {
        void BeforeInvoke(ILogger logger, MethodInfo method, object target, object[] arguments);

        void AfterInvoke(ILogger logger, MethodInfo method, object target, DateTime start, object result,
            Exception exception);

        void HandleException(ILogger logger, MethodInfo method, object target, Exception exception);
    }
}