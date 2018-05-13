using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFramework.Exceptions;

namespace DFramework.Infrastructure
{
    /// <summary>
    /// API响应结果
    /// </summary>
    public class ApiResult
    {
        public ApiResult()
        {
            Success = true;
            ErrorCode = 0;
        }

        public ApiResult(int errorCode, string message = null)
        {
            ErrorCode = errorCode;
            Message = message;
            Success = false;
        }

        /// <summary>
        /// API 执行是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// ErrorCode 为0 表示执行无异常
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// 当API执行有异常时，对应的错误信息
        /// </summary>
        public string Message { get; set; }
    }

    public class ApiResult<TResult> : ApiResult
    {
        public ApiResult()
        {
            Success = true;
        }

        public ApiResult(TResult result)
            : this()
        {
            Result = result;
        }

        public ApiResult(int errorCode, string message = null)
            : base(errorCode, message)
        {
        }

        /// <summary>
        /// API执行返回的结果
        /// </summary>
        public TResult Result { get; set; }
    }

    public class ExceptionManager
    {
        private static string GetExceptionMessage(Exception ex)
        {
#if DEBUG
            return $"Message:{ex.GetBaseException().Message}\r\nStackTrace:{ex.GetBaseException().StackTrace}";
#else
            return ex.GetBaseException().Message;
#endif
        }

        public static async Task<ApiResult<T>> ProcessAsync<T>(Func<Task<T>> func,
                                                                bool continueOnCapturedContext = false,
                                                                bool needRetry = false,
                                                                int retryCount = 50,
                                                                Func<Exception, string> getExceptionMessage = null)
        {
            ApiResult<T> apiResult = null;
            getExceptionMessage = getExceptionMessage ?? GetExceptionMessage;
            do
            {
                try
                {
                    var result = await func().ConfigureAwait(continueOnCapturedContext);
                    apiResult = new ApiResult<T>(result);
                    needRetry = false;
                }
                catch (Exception e)
                {
                    apiResult = new ApiResult<T>(ErrorCode.UnknownError, getExceptionMessage(e));
                    needRetry = false;
                }
            } while (needRetry && retryCount-- > 0);
            return apiResult;
        }

        public static async Task<ApiResult> ProcessAsync(Func<Task> func,
            bool continueOnCapturedContext = false,
            bool needRetry = false,
            int retryCount = 50,
            Func<Exception, string> getExceptionMessage = null)
        {
            getExceptionMessage = getExceptionMessage ?? GetExceptionMessage;
            ApiResult apiResult = null;
            do
            {
                try
                {
                    await func().ConfigureAwait(continueOnCapturedContext);
                    needRetry = false;
                    apiResult = new ApiResult();
                }
                catch (Exception e)
                {
                    apiResult = new ApiResult(ErrorCode.UnknownError, getExceptionMessage(e));
                    needRetry = false;
                }
            } while (needRetry && retryCount-- > 0);
            return apiResult;
        }

        public static ApiResult Process(Action action,
                                        bool needRetry = false,
                                        int retryCount = 50)
        {
            ApiResult apiResult = null;
            do
            {
                try
                {
                    action.Invoke();
                    apiResult = new ApiResult();
                }
                catch (Exception e)
                {
                    apiResult = new ApiResult(ErrorCode.UnknownError);
                }
            } while (needRetry && retryCount-- > 0);
            return apiResult;
        }

        public static ApiResult<T> Process<T>(Func<T> func,
            bool needRetry = false,
            int retryCount = 50,
            Func<Exception, string> getExceptionMessage = null)
        {
            ApiResult<T> apiResult = null;
            getExceptionMessage = getExceptionMessage ?? GetExceptionMessage;
            do
            {
                try
                {
                    var result = func();
                    needRetry = false;
                    apiResult = result != null ? new ApiResult<T>(result)
                                                : new ApiResult<T>();
                }
                catch (Exception e)
                {
                    apiResult = new ApiResult<T>(ErrorCode.UnknownError, getExceptionMessage(e));
                    needRetry = false;
                }
            } while (needRetry && retryCount-- > 0);
            return apiResult;
        }
    }
}