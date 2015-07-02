using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Llprk.Web.UI.Controllers.Results
{
    /// <summary>
    /// Liefert einen Json-String mit der Nachricht und dem Typ zurück.
    /// </summary>
    public class AjaxMessageResult : JsonResult
    {
        /// <summary>
        /// Arten von Nachrichten die über einen AJAX-Response angezeigt werden können.
        /// </summary>
        public enum AjaxMessageType { Success, Warning, Error, Info };

        /// <summary>
        /// Um welche Art von Nachricht handelt es sich hier?
        /// </summary>
        AjaxMessageType _Type;

        /// <summary>
        /// CTR
        /// </summary>
        /// <param name="message">Die anzuzeigende Nachricht.</param>
        public AjaxMessageResult(string message, AjaxMessageType type)
        {
            _Type = type;
            JsonRequestBehavior = System.Web.Mvc.JsonRequestBehavior.AllowGet;
            base.Data = new {
                type = type.ToString().ToLower(),
                message = message
            };
        }

        public override void ExecuteResult(ControllerContext context)
        {
            base.ExecuteResult(context);

            if (_Type == AjaxMessageType.Error) {
                // Housten...wir haben ein Problem.
                context.HttpContext.Response.StatusCode = 500;
            }
        }
    }
}