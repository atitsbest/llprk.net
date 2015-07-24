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
    public partial class ShopController : ApplicationController
    {
        private ThemeService _ThemeService;

        /// <summary>
        /// CTR
        /// </summary>
        /// <param name="themes"></param>
        public ShopController(ThemeService themes)
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
            var theme = _ThemeService.GetTheme(ViewBag.ThemeName);
            if (ViewBag.Unpublished != null) {
                theme = theme.Unpublished;
            }
            var layoutItem = theme.GetItem("layout.liquid", "layouts");
            var templateItem = theme.GetItem("index.liquid", "templates");

            Template.FileSystem = new LiquidFileSystem(theme);

            Template.RegisterFilter(typeof(ScriptTagFilter));
            Template.RegisterFilter(typeof(StylesheetTagFilter));
            Template.RegisterFilter(typeof(ImageUrlFilter));

            // Template lesen. TODO: Cache.
            var layout = Template.Parse(layoutItem.ReadContent());
            var template = Template.Parse(templateItem.ReadContent());

            // Render Template.
            template.Registers.Add("file_system", Template.FileSystem);
            var templateHtml = template.Render(Hash.FromAnonymousObject(new {
                products = viewModel.Products,
                page_title = "Index",
                template = "index"
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

    public class LiquidFileSystem : IFileSystem
    {
        private ITheme _Theme;

        public LiquidFileSystem(ITheme theme)
        {
            if (theme == null) throw new ArgumentNullException("theme");
            _Theme = theme;
        }

        public string ReadTemplateFile(Context context, string templateName)
        {
            var fileName = Path.HasExtension(templateName)
                ? templateName
                : Path.ChangeExtension(templateName, ".liquid");

            return _Theme.GetItem(fileName, "snippets").ReadContent();
        }
    }

}
