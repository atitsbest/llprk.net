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

namespace Llprk.Web.UI.Controllers
{
    public class ShopController : ApplicationController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int? id)
        {
            var viewModel = new ShopIndex();
            var categories = db.Categories.ToList();
            viewModel.Categories = categories.Select(c => {
                var products = db.Products
                       .Where(p => p.IsPublished
                                && p.CategoryId == c.Id
                                && p.Available > 0);
                if (!(id.HasValue && id.Value == c.Id)) {
                    // Auf der Startseite nur die ersten 3 anzeigen.
                    products = products.Take(3);
                }
                return new ShopCategory() {
                    Category = c,
                    Products = products.ToArray()
                };
            });
            return View(viewModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        public ActionResult Details(int id)
        {
            var viewModel = new ShopDetail();
            viewModel.Product = db.Products.Where(p => p.Id == id).FirstOrDefault();
            return View(viewModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public ActionResult New(OrderNew viewModel)
        {
            if (ModelState.IsValid) {
                var order = new Order() {
                    Address1 = viewModel.Address1,
                    Address2 = viewModel.Address2,
                    City = viewModel.City,
                    Country = db.Countries.First(c => c.Id == viewModel.CountryCode.ToLower()),
                    Email = viewModel.Email,
                    Firstname = viewModel.Firstname,
                    Name = viewModel.Name,
                    Salutation = viewModel.Salutation,
                    Zip = viewModel.Zip,
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

            return Json(null);
        }

    }
}
