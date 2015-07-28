using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Llprk.Web.UI.Areas.Admin.Models
{
    public class ShippingCostIndex
    {
        public class Country
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public int ShippingCostId { get; set; }
            public decimal ShippingCostAmount { get; set; }
            public decimal ShippingCostAdditionalAmount { get; set; }
        }

        public Country[] Countries { get; set; }
    }
}