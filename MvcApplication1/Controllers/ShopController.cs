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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
		[HttpPost]
        public ActionResult New(OrderNew viewModel)
        {
            System.Threading.Thread.Sleep(500);
            Order order = null;

            if (ModelState.IsValid) {
                order = new Order() {
                    Address1 = viewModel.Address1,
                    Address2 = viewModel.Address2,
                    City = viewModel.City,
                    Country = db.Countries.First(c => c.Id == viewModel.CountryCode.ToLower()),
                    Email = viewModel.Email,
                    Firstname = viewModel.Firstname,
                    Name = viewModel.Name,
                    Salutation = viewModel.Salutation,
                    Zip = viewModel.Zip,
                    Comment = viewModel.Comment
                };

                var productIdsAndQtys = viewModel.Products.ToDictionary(
                    k => k.Id,
                    v => v.Qty);

                try {
                    new ShopService().PlaceOrder(db, order, productIdsAndQtys);
                }
                catch (AppException e) {
                    Response.StatusCode = 500;
                    return Json(e.Message);
                }
            }
            else {
                Response.StatusCode = 500;
                return Json(ModelState.First().Value);
            }

            return Json(order.Id);
        }

    }
}
