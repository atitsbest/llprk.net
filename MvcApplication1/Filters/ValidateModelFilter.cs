using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Net;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Llprk.Web.UI.Filters
{
    public class ValidateModelFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var modelState = actionContext.ModelState;

            if (!modelState.IsValid) {
                actionContext.Response = actionContext.Request
                     .CreateErrorResponse(HttpStatusCode.BadRequest, modelState);
            }
        }
    }
}