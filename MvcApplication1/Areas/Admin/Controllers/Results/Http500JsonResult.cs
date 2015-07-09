using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace Llprk.Web.UI.Controllers.Results
{
    public class Http500JsonResult : JsonResult
    {

        protected class JsonReturnData
        {
            public string Message { get; set; }
            public object Data { get; set; }
        }

        public Http500JsonResult(string message, object data)
            : base()
        {
            this.Data = new JsonReturnData
            {
                Data = data,
                Message = message
            };
        }
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.StatusCode = 500;
            base.ExecuteResult(context);
        }
    }
}