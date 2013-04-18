using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Llprk.Web.UI.Models;

namespace Llprk.Web.UI.Controllers.Admin
{
    [Authorize]
    public class CountriesController : SingleValueController<Country, string>
    {
        public CountriesController()
            : base((e) => e.Countries)
        { }
    }
}