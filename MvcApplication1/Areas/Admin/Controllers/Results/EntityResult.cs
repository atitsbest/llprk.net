using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Web.Mvc;
using System.Web;

namespace Llprk.Web.UI.Controllers.Results
{
    public class EntityResult : JsonResult
    {
        protected class JsonReturnData
        {
            public string RedirectUrl { get; set; }
            public string Message { get; set; }
        }

        /// <summary>
        /// CTR
        /// </summary>
        public EntityResult()
            : base()
        {

            this.Data = new JsonReturnData();
        }

        /// <summary>
        /// CTR
        /// </summary>
        /// <param name="redirectUrl"></param>
        /// <param name="message"></param>
        public EntityResult(ActionResult actionResult, string message)
            : base()
        {
            var helper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            this.Data = new JsonReturnData {
                RedirectUrl = helper.Action(actionResult),
                Message = message
            };
        }

        /// <summary>
        /// CTR
        /// </summary>
        /// <param name="redirectUrl"></param>
        /// <param name="message"></param>
        public EntityResult(string redirectUrl, string message)
            : base()
        {
            this.Data = new JsonReturnData {
                RedirectUrl = redirectUrl,
                Message = message
            };
        }

        /// <summary>
        /// CTR - EntityResult without Redirect
        /// </summary>
        /// <param name="redirectUrl"></param>
        /// <param name="message"></param>
        public EntityResult(string message) //Manuel 20.05.2015 -> Für Auswahllisten, damit die Seite nicht immer neu geladen wird beim Update/Create
            : base()
        {
            this.Data = new JsonReturnData
            {
                Message = message
            };
        }

        /// <summary>
        /// Execute the result.
        /// </summary>
        /// <param name="context"></param>
        public override void ExecuteResult(ControllerContext context)
        {
            base.ExecuteResult(context);

            // In case we had an upload of files, that came through the iFrame to support IE9-...
            if ((context.HttpContext.Request.ContentType.ToLower().StartsWith("multipart/form-data"))) {
                context.HttpContext.Response.ContentType = "text/html";
            }

            //var message = ((JsonReturnData)this.Data).Message;
            //if (!string.IsNullOrEmpty(message))
            //{
            //    new Toastr().Success(message);
            //}
        }
    }
}


