using Llprk.Web.UI.Controllers.Results;
using Llprk.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Llprk.Web.UI.Controllers
{
    public class ApplicationController : Controller
    {
        protected Entities db = new Entities();


        /// <summary>
        /// Liefert ein ActionResult das einen Error anzeigt. Je nach Request Typ (Html, HtmlAsync, Json, ...)
        /// wird ein passendes Result erzeugt d.h. der Aufrufer muss sich nicht darum kümmern sondern
        /// kann beherzt <code>return Error("Fehlermeldung");</code> aufrufen.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected AjaxMessageResult Error(string message)
        {
            //log.Error(message);
            if (Request.IsAjaxRequest()) {
                return new AjaxMessageResult(message, AjaxMessageResult.AjaxMessageType.Error);
            }
            TempData.Add("error", message);
            return null;
        }

        /// <summary>
        /// Liefert ein ActionResult das einen Erfolg anzeigt. Je nach Request Typ (Html, HtmlAsync, Json, ...)
        /// wird ein passendes Result erzeugt d.h. der Aufrufer muss sich nicht darum kümmern sondern
        /// kann beherzt <code>return Error("Fehlermeldung");</code> aufrufen.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected AjaxMessageResult Success(string message)
        {
            TempData.Add("success", message);
            return new AjaxMessageResult(message, AjaxMessageResult.AjaxMessageType.Success);
        }

        /// <summary>
        /// Liefert ein ActionResult das eine Warnung anzeigt. Je nach Request Typ (Html, HtmlAsync, Json, ...)
        /// wird ein passendes Result erzeugt d.h. der Aufrufer muss sich nicht darum kümmern sondern
        /// kann beherzt <code>return Error("Fehlermeldung");</code> aufrufen.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected AjaxMessageResult Warning(string message)
        {
            return new AjaxMessageResult(message, AjaxMessageResult.AjaxMessageType.Warning);
        }

        /// <summary>
        /// Wie der Name schon sagt: Der Browser soll die Antwort auf keinen Fall cachen. 
        /// </summary>
        protected void DontCacheResponse()
        {
            dontCacheResponse(Response);
        }

        /// <summary>
        /// Http-Header mit "Kein-Cache" Einträgen anreichern.
        /// </summary>
        /// <param name="response"></param>
        public static void dontCacheResponse(HttpResponseBase response)
        {
            response.CacheControl = "no-cache";
            response.AddHeader("Pragma", "no-cache");
            response.Expires = -1;

            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.Cache.SetNoStore();
            response.Cache.SetExpires(DateTime.MinValue);
        }


        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}
