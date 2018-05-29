using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using DFramework.Infrastructure;

namespace DFramework.AspNet.IPRestriction
{
    public class IPFilterAttribute : ActionFilterAttribute
    {
        private readonly List<string> WhiteList;

        public IPFilterAttribute(string entry = null)
        {
            if (string.IsNullOrEmpty(entry))
            {
                WhiteList = IPRestrictExtension.IPRestrictConfig?.GlobalWhiteList;
            }
            else
            {
                WhiteList = IPRestrictExtension.IPRestrictConfig.EntryWhiteListDictionary.TryGetValue(entry, null);
            }

            WhiteList = WhiteList ?? new List<string>();
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);
            if (IPRestrictExtension.Enabled)
            {
                var clientIP = actionContext.Request.GetClientIP();
                if (clientIP != WebApiUtility.LocalIPv4
                    && clientIP != WebApiUtility.LocalIPv6
                    && !WhiteList.Contains(clientIP))
                {
                    throw new HttpResponseException(actionContext.Request
                        .CreateErrorResponse(HttpStatusCode.Forbidden,
                            WebApiUtility.ClientIPNotAllowedMessage));
                }
            }
        }
    }
}