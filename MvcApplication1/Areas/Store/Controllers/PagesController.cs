using Llprk.Web.UI.Areas.Store.Models;
using Llprk.Web.UI.Controllers;
using Llprk.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Llprk.Web.UI.Areas.Store.Controllers
{
    public partial class PagesController : ApplicationController
    {
        //
        // GET: /Pages/
        public virtual ActionResult Index()
        {
            return Content("");
        }
    }
}
