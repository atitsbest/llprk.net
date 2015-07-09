using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Llprk.DataAccess.Models;

namespace Llprk.Web.UI.Areas.Admin.Controllers
{
    [Authorize]
    public partial class CountriesController : SingleValueController<Country, string>
    {
        public CountriesController()
            : base((e) => e.Countries)
        { }
    }
}