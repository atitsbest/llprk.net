using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Llprk.Web.UI.Models;

namespace Llprk.Web.UI.Areas.Admin.Controllers
{
    [Authorize]
    public class CategoriesController : SingleValueController<Category, int>
    {
        public CategoriesController()
            : base((e) => e.Categories)
        { }
    }
}