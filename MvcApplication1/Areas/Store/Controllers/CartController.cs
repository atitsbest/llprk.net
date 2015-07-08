using DotLiquid;
using Llprk.Web.UI.Areas.Store.Models;
using Llprk.Web.UI.Controllers;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Llprk.Web.UI.ViewModels;
using System.Collections.Generic;
using Llprk.Web.UI.Liquid;

namespace Llprk.Web.UI.Areas.Store.Controllers
{
    public partial class CartController : ApplicationController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
#if !DEBUG
		[OutputCache(Duration=120, VaryByParam="id", NoStore=true)]
#endif
        public virtual ActionResult Index(int? id)
        {
            // Template-Name:
            var themeName = "minimal";
            var layoutName = "layout.liquid";
            var templateName = "index.liquid";
            var layoutPath = Server.MapPath(Path.Combine("Themes", themeName, layoutName));
            var templatePath = Server.MapPath(Path.Combine("Themes", themeName, templateName));

            Template.RegisterFilter(typeof(ScriptTagFilter));
            Template.RegisterFilter(typeof(StylesheetTagFilter));
            Template.RegisterFilter(typeof(ImageUrlFilter));

            // Template lesen. TODO: Cache.
            var layout = Template.Parse(System.IO.File.ReadAllText(layoutPath));
            var template = Template.Parse(System.IO.File.ReadAllText(templatePath));

            // Render Template.
            var templateHtml = template.Render(Hash.FromAnonymousObject(new
            {
                page_title = "Warenkorb",
                template = "cart"
            }));

            var layoutHtml = layout.Render(Hash.FromAnonymousObject(new
            {
                content_for_layout = templateHtml
            }));

            return Content(layoutHtml);
        }
    }

}
