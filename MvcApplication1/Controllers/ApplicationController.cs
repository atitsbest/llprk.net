using DotLiquid;
using Llprk.DataAccess.Models;
using Llprk.DataAccess.Models.Theme;
using Llprk.Web.UI.Areas.Store.Controllers;
using Llprk.Web.UI.Controllers.Results;
using Llprk.Web.UI.Liquid;
using System;
using System.Web;
using System.Web.Mvc;

namespace Llprk.Web.UI.Controllers
{
    public partial class ApplicationController : Controller
    {
        protected Entities db = new Entities();


        /// <summary>
        /// Setup all necessary to later render the shop templates.
        /// </summary>
        /// <param name="theme"></param>
        /// <param name="templateName"></param>
        /// <param name="layout"></param>
        /// <param name="template"></param>
        public void PrepareRenderTemplate(ITheme theme, string templateName, out Template layout, out Template template)
        {
            IThemeItem layoutItem;
            IThemeItem templateItem;
            if (ViewBag.Unpublished != null)
            {
                layoutItem = theme.GetUnpublishedItem("layout.liquid", "layouts");
                templateItem = theme.GetUnpublishedItem(templateName, "templates");
            }
            else
            {
                layoutItem = theme.GetItem("layout.liquid", "layouts");
                templateItem = theme.GetItem(templateName, "templates");
            }

            Template.FileSystem = new LiquidFileSystem(theme, ViewBag.Unpublished);

            Template.RegisterFilter(typeof(ScriptTagFilter));
            Template.RegisterFilter(typeof(StylesheetTagFilter));
            Template.RegisterFilter(typeof(ImageUrlFilter));

            // Template lesen. TODO: Cache.
            layout = Template.Parse(layoutItem.ReadContent());
            template = Template.Parse(templateItem.ReadContent());

            // Render Template.
            template.Registers.Add("file_system", Template.FileSystem);
        }

        /// <summary>
        /// Renders the Liquid-Template for the Shop.
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="templateHtml"></param>
        /// <returns></returns>
        public virtual ActionResult RenderTemplate(Template layout, string templateHtml)
        {
            var layoutHtml = layout.Render(Hash.FromAnonymousObject(new
            {
                content_for_layout = templateHtml,
                theme_designer_support = ViewBag.Unpublished ? @"
                    <script src=""/Scripts/jquery.signalR-2.2.0.min.js""></script>
                    <script src=""/signalr/hubs""></script>
                    <script>
                      $(function () {
                        var chat = $.connection.themeHub;
                        $.connection.hub.start();
                        chat.client.broadcastMessage = function (name, message) {
                          window.location.reload();
                        }
                      });
                    </script> " : null
            }));

            return Content(layoutHtml);
        }


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
        /// Json-Pendant für Json.Net.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected JsonNetResult JsonNet(object data)
        {
            return new JsonNetResult(data);
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
