using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Llprk.Web.UI.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ThemeFilterAttribute : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            var themeName = "minimal";

            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                themeName = themeName ?? HttpContext.Current.Request.QueryString["theme"];
                actionContext.Controller.ViewBag.Unpublished = HttpContext.Current.Request.QueryString["unpublished"] != null;
            }

            actionContext.Controller.ViewBag.ThemeName = themeName;
 	        base.OnActionExecuting(actionContext);
        }
    }
}