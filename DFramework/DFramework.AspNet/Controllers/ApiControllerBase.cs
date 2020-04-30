namespace DFramework.AspNet.Controllers
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.ModelBinding;

    using DFramework.Exceptions;
    using DFramework.Infrastructure;

    public class ApiControllerBase : ApiController
    {
        public ApiControllerBase(IExceptionManager exceptionManager)
        {
            this.ExceptionManager = exceptionManager;
        }

        protected IExceptionManager ExceptionManager { get; }

        public virtual string GetClientIP(HttpRequestMessage request = null)
        {
            return (request ?? this.Request).GetClientIP();
        }

        protected virtual string GetModelErrorMessage(ModelStateDictionary modelState)
        {
            return string.Join(
                ";",
                modelState.Where(m => (m.Value?.Errors?.Count ?? 0) > 0).Select(
                    m =>
                        $"{m.Key}:{string.Join(",", m.Value.Errors.Select(e => e.ErrorMessage + e.Exception?.Message))}"));
        }

        protected virtual ApiResult<T> Process<T>(
            Func<T> func,
            bool needRetry = true,
            Func<Exception, string> getExceptionMessage = null,
            Func<ModelStateDictionary, string> getModelErrorMessage = null)
        {
            if (this.ModelState.IsValid)
            {
                var apiResult = this.ExceptionManager.Process(
                    func,
                    needRetry,
                    getExceptionMessage: getExceptionMessage);
                return apiResult;
            }

            getModelErrorMessage = getModelErrorMessage ?? this.GetModelErrorMessage;
            return new ApiResult<T>(ErrorCode.InvalidParameters, getModelErrorMessage(this.ModelState));
        }

        protected virtual ApiResult Process(
            Action action,
            bool needRetry = true,
            Func<Exception, string> getExceptionMessage = null,
            Func<ModelStateDictionary, string> getModelErrorMessage = null)
        {
            if (this.ModelState.IsValid)
            {
                var apiResult = this.ExceptionManager.Process(
                    action,
                    needRetry,
                    getExceptionMessage: getExceptionMessage);
                return apiResult;
            }

            getModelErrorMessage = getModelErrorMessage ?? this.GetModelErrorMessage;
            return new ApiResult(ErrorCode.InvalidParameters, getModelErrorMessage(this.ModelState));
        }

        protected virtual async Task<ApiResult> ProcessAsync(
            Func<Task> func,
            bool continueOnCapturedContext = false,
            bool needRetry = true,
            Func<Exception, string> getExceptionMessage = null,
            Func<ModelStateDictionary, string> getModelErrorMessage = null)
        {
            if (this.ModelState.IsValid)
                return await this.ExceptionManager.ProcessAsync(
                           func,
                           continueOnCapturedContext,
                           needRetry,
                           getExceptionMessage: getExceptionMessage).ConfigureAwait(continueOnCapturedContext);
            getModelErrorMessage = getModelErrorMessage ?? this.GetModelErrorMessage;
            return new ApiResult(ErrorCode.InvalidParameters, getModelErrorMessage(this.ModelState));
        }

        protected virtual async Task<ApiResult<T>> ProcessAsync<T>(
            Func<Task<T>> func,
            bool continueOnCapturedContext = false,
            bool needRetry = true,
            Func<Exception, string> getExceptionMessage = null,
            Func<ModelStateDictionary, string> getModelErrorMessage = null)
        {
            if (this.ModelState.IsValid)
                return await this.ExceptionManager.ProcessAsync(
                           func,
                           continueOnCapturedContext,
                           needRetry,
                           getExceptionMessage: getExceptionMessage).ConfigureAwait(continueOnCapturedContext);
            getModelErrorMessage = getModelErrorMessage ?? this.GetModelErrorMessage;
            return new ApiResult<T>(ErrorCode.InvalidParameters, getModelErrorMessage(this.ModelState));
        }
    }
}