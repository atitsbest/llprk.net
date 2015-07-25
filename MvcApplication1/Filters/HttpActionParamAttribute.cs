using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Llprk.Web.UI.Filters
{
    /// <summary>
    /// Actions decorated with it:
    ///
    /// [HttpParamAction]
    /// public ActionResult Save(MyModel model)
    /// {
    ///     // ...
    /// }
    /// 
    /// [HttpParamAction]
    /// public ActionResult Publish(MyModel model)
    /// {
    ///     // ...
    /// }
    ///
    /// HTML/Razor
    /// 
    /// @using (@Html.BeginForm())
    /// {
    ///     <!-- form content here -->
    ///     <input type="submit" name="Save" value="Save" />
    ///     <input type="submit" name="Publish" value="Publish" />
    /// }
    /// 
    /// </summary>
    public class HttpParamActionAttribute : ActionNameSelectorAttribute
    {
        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            if (actionName.Equals(methodInfo.Name, StringComparison.InvariantCultureIgnoreCase))
                return true;

            var request = controllerContext.RequestContext.HttpContext.Request;
            return request[methodInfo.Name] != null;
        }
    }
}