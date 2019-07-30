using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyMvcTest
{
    public class CsrfFilterAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (filterContext.RequestContext.HttpContext.Request.HttpMethod.Equals("POST",
                comparisonType: StringComparison.OrdinalIgnoreCase))
            {
                var urlReferrerHost = filterContext.RequestContext.HttpContext.Request.UrlReferrer?.Host;
                if (!string.IsNullOrEmpty(urlReferrerHost))
                {
                    var urlHost = filterContext.RequestContext.HttpContext.Request.Url?.Host;
                    if (!string.IsNullOrEmpty(urlHost) && urlHost != urlReferrerHost)
                    {
                        filterContext.Result = new RedirectResult("http://www.baidu.com");
                    }
                }
            }
            
            base.OnActionExecuting(filterContext);
        }
    }
}