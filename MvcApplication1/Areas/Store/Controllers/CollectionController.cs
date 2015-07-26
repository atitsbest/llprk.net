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
using DotLiquid.FileSystems;
using System;
using Llprk.Application.Services;
using Llprk.Web.UI.Filters;
using Llprk.DataAccess.Models.Theme;

namespace Llprk.Web.UI.Areas.Store.Controllers
{
    [ThemeFilter]
    public partial class CollectionController : ApplicationController
    {
        private ThemeService _ThemeService;

        /// <summary>
        /// CTR
        /// </summary>
        /// <param name="themes"></param>
        public CollectionController(ThemeService themes)
        {
            if (themes == null) throw new ArgumentNullException("themes");

            _ThemeService = themes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
#if !DEBUG
		[OutputCache(Duration=120, VaryByParam="id", NoStore=true)]
#endif
        [ThemeFilter]
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

            // Templating:
            Template layout;
            Template template;
            var theme = _ThemeService.GetTheme(ViewBag.ThemeName);
            PrepareRenderTemplate(theme, "collection.liquid", out layout, out template);

            var templateHtml = template.Render(Hash.FromAnonymousObject(new {
                products = viewModel.Products,
                page_title = "Collection",
                template = "collection"
            }));

            var layoutHtml = layout.Render(Hash.FromAnonymousObject(new {
                content_for_layout = templateHtml
            }));

            return Content(layoutHtml);
        }

    }

}
