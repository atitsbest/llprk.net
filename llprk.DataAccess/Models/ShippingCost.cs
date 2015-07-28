using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Llprk.DataAccess.Models
{
    public class ShippingCost
    {
        public string CountryId { get; set; }
		public virtual Country Country { get; set; } 

        public int ShippingCategoryId  { get; set; }
        public virtual ShippingCategory ShippingCategory { get; set; }

        public decimal Amount { get; set; }
        public decimal AdditionalAmount { get; set; }
    }
}