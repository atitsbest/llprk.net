using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Llprk.DataAccess.Models;
using DotLiquid;
using Llprk.Web.UI.ViewModels;
using System.Web.Mvc;

namespace Llprk.Web.UI.Areas.Store.Models
{
    /// <summary>
    /// Update Checkout variable costs.
    /// </summary>
    public class CheckoutVariableCosts
    {
        public string ShippingCosts { get; set; }
        public string Tax { get; set; }
        public string Total { get; set; }
    }
}