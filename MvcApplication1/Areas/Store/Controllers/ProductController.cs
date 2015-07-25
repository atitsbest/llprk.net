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
    public partial class ProductController : ApplicationController
    {
        private ThemeService _ThemeService;

        /// <summary>
        /// CTR
        /// </summary>
        /// <param name="themes"></param>
        public ProductController(ThemeService themes)
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
        public virtual ActionResult Index(int id)
        {
            var viewModel = new ProductIndex
            {
                Product = Mapper.Map<LiquidProduct>(db.Products
                        .Single(p => p.IsPublished
                                && p.Id == id
                                && p.Available > 0)) // Nur verfügbare Produkte anzeigen.
            };

            // Templating:
            var theme = _ThemeService.GetTheme(ViewBag.ThemeName);
            IThemeItem layoutItem;
            IThemeItem templateItem;
            if (ViewBag.Unpublished != null)
            {
                layoutItem = theme.GetUnpublishedItem("layout.liquid", "layouts");
                templateItem = theme.GetUnpublishedItem("product.liquid", "templates");
            }
            else
            {
                layoutItem = theme.GetItem("layout.liquid", "layouts");
                templateItem = theme.GetItem("product.liquid", "templates");
            }

            Template.FileSystem = new LiquidFileSystem(theme, ViewBag.Unpublished);

            Template.RegisterFilter(typeof(ScriptTagFilter));
            Template.RegisterFilter(typeof(StylesheetTagFilter));
            Template.RegisterFilter(typeof(ImageUrlFilter));

            // Template lesen. TODO: Cache.
            var layout = Template.Parse(layoutItem.ReadContent());
            var template = Template.Parse(templateItem.ReadContent());

            // Render Template.
            template.Registers.Add("file_system", Template.FileSystem);
            var templateHtml = template.Render(Hash.FromAnonymousObject(new {
                product = viewModel.Product,
                add_to_cart_url = viewModel.AddToCartUrl,
                page_title = "Product",
                template = "product"
            }));

            return RenderTemplate(layout, templateHtml);
        }

    }

}
