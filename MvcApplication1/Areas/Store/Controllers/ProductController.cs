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
            Template layout;
            Template template;
            var theme = _ThemeService.GetTheme(ViewBag.ThemeName);
            PrepareRenderTemplate(theme, "product.liquid", out layout, out template);

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
