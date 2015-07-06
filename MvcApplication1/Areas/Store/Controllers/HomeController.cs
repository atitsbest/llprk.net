using Llprk.Web.UI.Areas.Store.Models;
using Llprk.Web.UI.Controllers;
using Llprk.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Llprk.Web.UI.Areas.Store.Controllers
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
#if !DEBUG
		[OutputCache(Duration=120, NoStore=true)]
#endif
        public ActionResult About()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
#if !DEBUG
		[OutputCache(Duration=120, NoStore=true)]
#endif
        public ActionResult ShowYourLove()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
#if !DEBUG
		[OutputCache(Duration=120, NoStore=true)]
#endif
        public ActionResult Impressum()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
#if !DEBUG
		[OutputCache(Duration=120, NoStore=true)]
#endif
        public ActionResult Agbs()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
#if !DEBUG
		[OutputCache(Duration=120, NoStore=true)]
#endif
        public ActionResult Widerrufsrecht()
        {
            return View();
        }

    }
}
