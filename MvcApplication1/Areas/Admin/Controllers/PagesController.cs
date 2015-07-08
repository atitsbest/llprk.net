using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Llprk.Web.UI.Models;
using AutoMapper;
using Llprk.Web.UI.Application;
using Llprk.Web.UI.Controllers;
using Llprk.Web.UI.Areas.Admin.Models;

namespace Llprk.Web.UI.Areas.Admin.Controllers
{
    [Authorize]
    public partial class PagesController : ApplicationController
    {
        //
        // GET: /Pages/
        public virtual ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Pages/Details/5
        public virtual ActionResult Details(int id = 0)
        {
            return View();
        }

        //
        // GET: /Pages/Delete/5
        public virtual ActionResult Delete(int id = 0)
        {
            return View();
        }

        //
        // POST: /Pages/Delete/5

        [HttpPost, ActionName("Delete")]
        public virtual ActionResult DeleteConfirmed(int id)
        {
            return View();
        }
    }
}