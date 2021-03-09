using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace cns.Services.App
{
    /// <summary>
    /// Session檢查
    /// </summary>
    [Serializable]
    public class ActionCheck : Microsoft.AspNetCore.Mvc.Filters.ActionFilterAttribute
    {
        public ActionCheck()
        {
        }

        // Action執行的之前，判斷Session
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            byte[] by = new byte[10];
            if (!filterContext.HttpContext.Session.TryGetValue(SessionKey.usrInfo, out by))
            {
                filterContext.Result = new RedirectToRouteResult("Default",
                    new RouteValueDictionary(new { controller = "Home", action = "Loginoff", returnurl = Microsoft.AspNetCore.Http.Extensions.UriHelper.GetEncodedUrl(filterContext.HttpContext.Request) }));

            }
        }
    }
}
