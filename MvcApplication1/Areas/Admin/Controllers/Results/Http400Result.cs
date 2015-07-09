using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Web.Mvc;
using System.Web.Http;

namespace Llprk.Web.UI.Controllers.Results
{
    public class Http400Result : ActionResult
    {
        private ModelStateDictionary _ModelState;

        /// <summary>
        /// CTR
        /// </summary>
        /// <remarks></remarks>
        public Http400Result()
        {
        }

        /// <summary>
        /// CTR
        /// </summary>
        /// <param name="modelstate"></param>
        /// <remarks></remarks>
        public Http400Result(ModelStateDictionary modelstate)
        {
            _ModelState = modelstate;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.StatusCode = 400;
            if (_ModelState != null) {
                dynamic str = JsonConvert.SerializeObject(_ModelState, GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings);
                dynamic res = context.HttpContext.Response;
                dynamic isMultipart = context.HttpContext.Request.ContentType.ToLower().StartsWith("multipart/form-data");

                res.ContentType = "application/json";

                // In case we had an upload of files, that came through the iFrame to support IE9-...
                if ((isMultipart)) {
                    res.ContentType = "text";
                    res.Write("<400>");
                    // Status - Code.
                }

                res.Write(str);

                // In case we had an upload of files, that came through the iFrame to support IE9-...
                if ((isMultipart)) {
                    res.Write("</400>");
                    // Status - Code.
                }
            }
        }
    }

}