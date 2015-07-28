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
    public partial class TaxesController : ApplicationController
    {
        private ITaxService _TaxService;

        /// <summary>
        /// CTR
        /// </summary>
        public TaxesController(ITaxService taxes)
        {
            if (taxes == null) throw new ArgumentNullException("taxes");

            _TaxService = taxes;
        }


        /// <summary>
        /// Show Tax settings and taxes.
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Index()
        {
            var db = new Entities();
            var vm = new TaxIndex
            {
                Countries = Mapper.Map<TaxIndex.Country[]>(db.Countries)
            };
            return View(vm);
        }

        /// <summary>
        /// Update taxes for a single country.
        /// </summary>
        /// <param name="country"></param>
        /// <param name="percent"></param>
        /// <returns></returns>
        public virtual JsonResult Update(string country, int percent)
        {
            _TaxService.UpdateCountryTax(country, percent);
            return new EntityResult(string.Format("Changed tax for {0} to {1}%.", country, percent));
        }
    }
}