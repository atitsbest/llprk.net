using Llprk.Web.UI.Models;
using Llprk.Web.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Llprk.Web.UI.Application;
using Llprk.Web.UI.Application.Exceptions;
using System.Globalization;
using System.IO;

namespace Llprk.Web.UI.Controllers
{
    public class ShopController : ApplicationController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
#if !DEBUG
		[OutputCache(Duration=120, VaryByParam="id", NoStore=true)]
#endif
        public ActionResult Index(int? id)
        {
            var viewModel = new ShopIndex();
            viewModel.Categories = db.Categories.ToList();
            viewModel.Products = db.Products
						.Where(p => p.IsPublished
                                && (!id.HasValue || (p.CategoryId == id.Value))
                                && p.Available > 0) // Nur verfügbare Produkte anzeigen.
						.OrderBy(p => p.CreatedAt)
						.ToArray();
			var di = new DirectoryInfo(Server.MapPath("~/Images/marketing/"));
			viewModel.BannerUrls = di.GetFiles("banner*.*").Select(s => s.Name).OrderBy(s => s).ToArray();
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
        public ActionResult Categories(int id)
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
        public ActionResult Details(int id)
        {
            var viewModel = new ShopDetail();
            viewModel.Product = db.Products
				.Include(p => p.Category)
                .Single(p => p.Id == id);
            return View(viewModel);
        }

    }
}
