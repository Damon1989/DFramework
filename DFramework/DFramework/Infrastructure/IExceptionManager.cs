using System;
using System.Threading.Tasks;

namespace DFramework.Infrastructure
{
    public interface IExceptionManager
    {
        Task<ApiResult<T>> ProcessAsync<T>(Func<Task<T>> func,
                                           bool continueOnCapturedContext = false,
                                           bool needRetry = false,
                                           int retryCount = 50,
                                           Func<Exception, string> getExceptionMessage = null);

        Task<ApiResult> ProcessAsync(Func<Task> func,
                                     bool continueOnCapturedContext = false,
                                     bool needRetry = false,
                                     int retryCount = 50,
                                     Func<Exception, string> getExceptionMessage = null);

        ApiResult Process(Action action,
                          bool needRetry = false,
                          int retryCount = 50,
                          Func<Exception, string> getExceptionMessage = null);

        ApiResult<T> Process<T>(Func<T> func,
                                bool needRetry = false,
                                int retryCount = 50,
                                Func<Exception, string> getExceptionMessage = null);
    }
}