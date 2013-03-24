using Llprk.Web.UI.Models;
using Llprk.Web.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Llprk.Web.UI.Controllers
{
    public class HomeController : ApplicationController
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View(new HomeIndex() {
                Products = db.Products.Take(4).ToArray()
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Blog(int id) {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowYourLove()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Impressum()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Agbs()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Widerrufsrecht()
        {
            return View();
        }

    }
}
