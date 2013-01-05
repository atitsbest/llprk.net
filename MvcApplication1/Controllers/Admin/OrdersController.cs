using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication1.Models;
using MvcApplication1.ViewModels;
using AutoMapper;

namespace MvcApplication1.Controllers.Admin
{
    [Authorize]
    public class OrdersController : Controller
    {
        private ShopDb db = new ShopDb();

        //
        // GET: /Orders/
        public ActionResult Index()
        {
            var result = db.Orders.Include(i => i.OrderLines).ToList();
            return View(result);
        }

        //
        // GET: /Orders/Details/5
        public ActionResult Details(int id = 0)
        {
            var order = db.Orders
                .Include(i => i.OrderLines)
                .Where(p => p.Id == id)
                .FirstOrDefault();
            if (order == null) {
                return HttpNotFound();
            }
            return View(order);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}