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
    public partial class ShopController : ApplicationController
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
            var viewModel = new ShopIndex();
            viewModel.Categories = db.Categories.ToList();
            viewModel.Products = Mapper.Map<IEnumerable<LiquidProduct>>(db.Products
						.Where(p => p.IsPublished
                                && (!id.HasValue || (p.CategoryId == id.Value))
                                && p.Available > 0) // Nur verfügbare Produkte anzeigen.
						.OrderBy(p => p.CreatedAt)
						.ToArray());
			var di = new DirectoryInfo(Server.MapPath("~/Images/marketing/"));
			viewModel.BannerUrls = di.GetFiles("banner*.*").Select(s => s.Name).OrderBy(s => s).ToArray();

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
            var templateHtml = template.Render(Hash.FromAnonymousObject(new {
                products = viewModel.Products
            }));

            var layoutHtml = layout.Render(Hash.FromAnonymousObject(new {
                content_for_layout = templateHtml
            }));

            return Content(layoutHtml);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
#if !DEBUG
		[OutputCache(Duration=120, VaryByParam="id", NoStore=true)]
#endif
        public virtual ActionResult Categories(int id)
        {
            var viewModel = new ShopCategory();
            viewModel.Category = db.Categories.First(c => c.Id == id);
            viewModel.Products = db.Products
                       .Where(p => p.IsPublished
                                && p.CategoryId == id
                                && p.Available > 0) // Nur verfügbare Produkte anzeigen.
                       .ToArray();
            return View(viewModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
#if !DEBUG
		[OutputCache(Duration=120, VaryByParam="id", NoStore=true)]
#endif
        public virtual ActionResult Details(int id)
        {
            var viewModel = new ShopDetail();
            viewModel.Product = db.Products
				.Include(p => p.Category)
                .Single(p => p.Id == id);
            return View(viewModel);
        }

    }
}
