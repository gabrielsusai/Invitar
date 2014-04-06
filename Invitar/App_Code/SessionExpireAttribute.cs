using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace Invitar
{
    public class SessionExpireAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;

            if (ctx != null)
            {
                if (ctx.Session["UserId"] == null || !ctx.Request.IsAuthenticated)
                {
                    if (ctx.Request.IsAuthenticated)
                    {
                        ctx.GetOwinContext().Authentication.SignOut();
                    }
                    //filterContext.Controller.TempData.Add("message", "Your session timedout!");
                    filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                        { "Controller", "Account" },
                        { "Action", "Login" }
                });
                    return;
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}