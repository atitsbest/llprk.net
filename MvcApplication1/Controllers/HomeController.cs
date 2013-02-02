using Llprk.Web.UI.Models;
using Llprk.Web.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Llprk.Web.UI.Controllers
{
    public class HomeController : Controller
    {
        private Entities db = new Entities();
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View(new HomeIndex() { 
                Products = db.Products.Take(4)
            });
        }


        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
