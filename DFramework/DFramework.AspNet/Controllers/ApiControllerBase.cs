using DFramework.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using DFramework.Exceptions;

namespace DFramework.AspNet.Controllers
{
    public class ApiControllerBase : ApiController
    {
        protected IExceptionManager ExceptionManager { get; }

        public ApiControllerBase(IExceptionManager exceptionManager)
        {
            ExceptionManager = exceptionManager;
        }

        protected virtual string GetModelErrorMessage(ModelStateDictionary modelState)
        {
            return string.Join(";", modelState.Where(m => (m.Value?.Errors?.Count ?? 0) > 0)
                .Select(m =>
                    $"{m.Key}:{string.Join(",", m.Value.Errors.Select(e => e.ErrorMessage + e.Exception?.Message))}"));
        }

        protected virtual ApiResult<T> Process<T>(Func<T> func,
            bool needRetry = true,
            Func<Exception, string> getExceptionMessage = null,
            Func<ModelStateDictionary, string> getModelErrorMessage = null)
        {
            if (ModelState.IsValid)
            {
                var apiResult = ExceptionManager.Process(func, needRetry,
                    getExceptionMessage: getExceptionMessage);
                return apiResult;
            }
            getModelErrorMessage = getModelErrorMessage ?? GetModelErrorMessage;
            return new ApiResult<T>(
                ErrorCode.InvalidParameters,
                getModelErrorMessage(ModelState)
                );
        }

        protected virtual ApiResult Process(Action action,
            bool needRetry = true,
            Func<Exception, string> getExceptionMessage = null,
            Func<ModelStateDictionary, string> getModelErrorMessage = null)
        {
            if (ModelState.IsValid)
            {
                var apiResult = ExceptionManager.Process(action, needRetry, getExceptionMessage: getExceptionMessage);
                return apiResult;
            }
            getModelErrorMessage = getModelErrorMessage ?? GetModelErrorMessage;
            return
                new ApiResult
                (
                    ErrorCode.InvalidParameters,
                    getModelErrorMessage(ModelState)
                );
        }

        protected virtual async Task<ApiResult> ProcessAsync(Func<Task> func,
                                                     bool continueOnCapturedContext = false,
                                                     bool needRetry = true,
                                                     Func<Exception, string> getExceptionMessage = null,
                                                     Func<ModelStateDictionary, string> getModelErrorMessage = null)
        {
            if (ModelState.IsValid)
            {
                return await ExceptionManager.ProcessAsync(func,
                                                           continueOnCapturedContext,
                                                           needRetry,
                                                           getExceptionMessage: getExceptionMessage)
                                             .ConfigureAwait(continueOnCapturedContext);
            }
            getModelErrorMessage = getModelErrorMessage ?? GetModelErrorMessage;
            return
                new ApiResult
                    (
                     ErrorCode.InvalidParameters,
                     getModelErrorMessage(ModelState)
                    );
        }

        protected virtual async Task<ApiResult<T>> ProcessAsync<T>(Func<Task<T>> func,
                                                                   bool continueOnCapturedContext = false,
                                                                   bool needRetry = true,
                                                                   Func<Exception, string> getExceptionMessage = null,
                                                                   Func<ModelStateDictionary, string> getModelErrorMessage = null)
        {
            if (ModelState.IsValid)
            {
                return await ExceptionManager.ProcessAsync(func, continueOnCapturedContext, needRetry,
                                                           getExceptionMessage: getExceptionMessage)
                                             .ConfigureAwait(continueOnCapturedContext);
            }
            getModelErrorMessage = getModelErrorMessage ?? GetModelErrorMessage;
            return
                new ApiResult<T>
                    (
                     ErrorCode.InvalidParameters,
                     getModelErrorMessage(ModelState)
                    );
        }
    }
}