using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace Llprk.Web.UI.Controllers.Results
{
    public class Http500Result : ActionResult
    {
        public string ResponseText { get; set; }

        public Http500Result(string responseText)
        {
            this.ResponseText = responseText;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.StatusCode = 500;
            context.HttpContext.Response.ContentType = "text/plain";
            context.HttpContext.Response.Write(this.ResponseText);
        }
    }
}