using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Llprk.DataAccess.Models;
using Llprk.Web.UI.Controllers;
using Llprk.Web.UI.Controllers.Results;
using Llprk.Application.Services;
using Llprk.Web.UI.Areas.Admin.Models;
using AutoMapper;

namespace Llprk.Web.UI.Areas.Admin.Controllers
{
    [Authorize]
    public partial class DashboardController : ApplicationController
    {
        /// <summary>
        /// CTR
        /// </summary>
        public DashboardController()
        {
        }


        /// <summary>
        /// Show Tax settings and taxes.
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Index()
        {
            // Haben alle Produkte eine ShippingCategory?
            // Sind alle ShippingCosts gesetzt?
            // Ist Paypal konfiguriert?
            // Ist E-Mail konfiguriert?
            // Wenn ein Produkt Steuern verwendet: Sind Steuern für alle Länder gesetzt?
            return View();
        }
    }
}