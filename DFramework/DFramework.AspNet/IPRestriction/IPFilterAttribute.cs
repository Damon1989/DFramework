using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using DFramework.Infrastructure;

namespace DFramework.AspNet.IPRestriction
{
    public class IPFilterAttribute : ActionFilterAttribute
    {
        private readonly List<string> _whiteList;

        public IPFilterAttribute(string entry = null)
        {
            _whiteList = string.IsNullOrEmpty(entry) ? IPRestrictExtension.IPRestrictConfig?.GlobalWhiteList
                                                    : IPRestrictExtension.IPRestrictConfig.EntryWhiteListDictionary.TryGetValue(entry, null);

            _whiteList = _whiteList ?? new List<string>();
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);
            if (!IPRestrictExtension.Enabled) return;
            var clientIp = actionContext.Request.GetClientIP();
            if (clientIp != WebApiUtility.LocalIPv4
                && clientIp != WebApiUtility.LocalIPv6
                && !_whiteList.Contains(clientIp))
            {
                throw new HttpResponseException(actionContext.Request
                    .CreateErrorResponse(HttpStatusCode.Forbidden,
                        WebApiUtility.ClientIPNotAllowedMessage));
            }
        }
    }
}