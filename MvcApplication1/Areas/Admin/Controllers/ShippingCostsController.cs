using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Llprk.DataAccess.Models;
using System.Text.RegularExpressions;
using Llprk.Web.UI.Controllers;
using Llprk.Web.UI.Areas.Admin.Models;
using AutoMapper;
using Llprk.Web.UI.Controllers.Results;
using Llprk.Application.Services;
using Llprk.Application.DTOs.Requests;

namespace Llprk.Web.UI.Areas.Admin.Controllers
{
    [Authorize]
    public partial class ShippingCostsController : ApplicationController
    {
        private IShippingService _ShippingService;

        /// <summary>
        /// CTR
        /// </summary>
        public ShippingCostsController(IShippingService shipping)
        {
            if (shipping == null) throw new ArgumentNullException("shipping");

            _ShippingService = shipping;
        }

        /// <summary>
        /// Show shipping costs.
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Index() 
        {
            using (var db = new Entities())
            {
                var vm = new ShippingCostIndex
                {
                    Countries = Mapper.Map<ShippingCostIndex.Country[]>(db.Countries),
                    ShippingCategories = Mapper.Map<ShippingCostIndex.ShippingCategory[]>(db.ShippingCategories)
                };
                return View(vm);
            }
        }

        /// <summary>
        /// Update shipping costs.
        /// </summary>
        /// <returns></returns>
        public virtual JsonResult Update(ShippingCostUpdate data) 
        {
            _ShippingService.UpdateShippingCosts(data);
            return new EntityResult("Updated shipping costs.");
        }

        /// <summary>
        /// Change shipping category name
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual JsonResult UpdateShippingCategory(int id, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                _ShippingService.DeleteShippingCost(id);
                return new EntityResult("Deleted " + name);
            }
            else
            {
                _ShippingService.UpdateShippingCategory(id, name);
                return new EntityResult("Changed name to " + name);
            }
        }

        /// <summary>
        /// Change shipping category name
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual JsonResult CreateShippingCategory(string name)
        {
            var entity = _ShippingService.CreateShippingCategory(name);
            return JsonNet(Mapper.Map<ShippingCostIndex.ShippingCategory>(entity));
        }
    }
}