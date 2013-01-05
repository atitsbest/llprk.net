using MvcApplication1.Models;
using MvcApplication1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.Controllers
{
    public class ShopController : Controller
    {
        public ActionResult Index()
        {
            var viewModel = new ShopIndex();
            using (var db = new ShopDb()) {
                var categories = db.Categories.ToList();
                viewModel.Categories = categories.ToDictionary(c => c.Name, 
                                                               c => db.Products
                                                                       .Include(i => i.Pictures)
                                                                       .Where(p => p.IsPublished && p.CategoryId == c.Id).ToArray());
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
                    var order = new Order();
                    order.Address1 = viewModel.Address1;
                    order.Address2 = viewModel.Address2;
                    order.City = viewModel.City;
                    order.CountryCode = "at"; // TODO: Land übergeben.
                    order.Email = viewModel.Email;
                    order.Firstname = viewModel.Firstname;
                    order.Name = viewModel.Name;
                    order.Salutation = viewModel.Salutation;
                    order.Zip = viewModel.Zip;
                    db.Orders.Add(order);
                    db.SaveChanges();

                    foreach (var p in viewModel.Products) {
                        var product = db.Products.FirstOrDefault(x => x.Id == p.Id);
                        if (product == null) {
                            ModelState.AddModelError("products", string.Format("Das Produkt mit der Id {0} ist nicht verfügbar!", p.Id));
                        }
                        order.OrderLines.Add(new OrderLine() {
                            OrderId = order.Id,
                            ProductId = p.Id,
                            Qty = p.Qty
                        });
                    }
                    db.SaveChanges();
                }
                return Json("");
            }

            return Json("");
        }

    }
}
