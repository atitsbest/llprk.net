using MvcApplication1.Models;
using MvcApplication1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using MvcApplication1.Application;
using MvcApplication1.Application.Exceptions;

namespace MvcApplication1.Controllers
{
    public class ShopController : Controller
    {
        public ActionResult Index()
        {
            var viewModel = new ShopIndex();
            using (var db = new ShopDb()) {
                var categories = db.Categories.ToList();
                viewModel.Categories = categories.ToDictionary(
                    c => c.Name, 
                    c => db.Products
                           .Include(i => i.Pictures)
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
            using(var db = new ShopDb()) {
                viewModel.Product = db.Products.Include(i => i.Pictures).Where(p => p.Id == id).FirstOrDefault();
            }
            return View(viewModel);
        }

        public ActionResult New(OrderNew viewModel)
        {
            if (ModelState.IsValid) {
                using (var db = new ShopDb()) {
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
