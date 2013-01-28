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
    public class ShopController : Controller
    {
        public ActionResult Index()
        {
            var viewModel = new ShopIndex();
            using (var db = new Entities()) {
                var categories = db.Categories.ToList();
                viewModel.Categories = categories.ToDictionary(
                    c => c.Name, 
                    c => db.Products
                           .Where(p => p.IsPublished 
                                    && p.CategoryId == c.Id
                                    && p.Available > 0) // Nur verfügbare Produkte anzeigen.
                           .ToArray());
            }
            return View(viewModel);
        }

        public ActionResult Details(int id)
        {
            var viewModel = new ShopDetail();
            using(var db = new Entities()) {
                viewModel.Product = db.Products.Where(p => p.Id == id).FirstOrDefault();
            }
            return View(viewModel);
        }

        public ActionResult New(OrderNew viewModel)
        {
            if (ModelState.IsValid) {
                using (var db = new Entities()) {
                    var order = new Order() {
                        Address1 = viewModel.Address1,
                        Address2 = viewModel.Address2,
                        City = viewModel.City,
                        CountryCode = "at", // TODO: Land übergeben.
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
            }
            else {
                Response.StatusCode = 500;
                return Json(ModelState.First().Value);
            }

            return Json(null);
        }

    }
}
