using Llprk.Application.DTOs.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Llprk.Web.UI.Areas.Admin.Models
{
    public class ShippingCostIndex : UpdateShippingCostsRequest
    {
        public class ShippingCategory
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public Country[] Countries { get; set; }
        public ShippingCategory[] ShippingCategories { get; set; }

        public string UpdateShippingCategoryUrl;
        public string CreateShippingCategoryUrl;
        public string UpdateShippingCostsUrl;


        /// <summary>
        /// CTR
        /// </summary>
        public ShippingCostIndex()
        {
            var url = new UrlHelper(HttpContext.Current.Request.RequestContext);
            UpdateShippingCategoryUrl = url.Action(MVC.Admin.ShippingCosts.UpdateShippingCategory());
            CreateShippingCategoryUrl = url.Action(MVC.Admin.ShippingCosts.CreateShippingCategory());
            UpdateShippingCostsUrl = url.Action(MVC.Admin.ShippingCosts.Update());
        }
    }
}