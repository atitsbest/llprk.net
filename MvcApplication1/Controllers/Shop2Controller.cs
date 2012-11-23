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
    public class Shop2Controller : Controller
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

        public ActionResult Products()
        {
            return View();
        }

        public ActionResult Detail()
        {
            return View();
        }

        public ActionResult Cart()
        {
            return View();
        }
    }
}
