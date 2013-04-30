using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Llprk.Web.UI.Models
{
    public class ShippingCost
    {
        public string CountryId { get; set; }
        [ScriptIgnore, JsonIgnore]
		public virtual Country Country { get; set; } 

        public int ShippingCategoryId  { get; set; }
		[ScriptIgnore, JsonIgnore]
        public virtual ShippingCategory ShippingCategory { get; set; }

        public decimal Amount { get; set; }
    }
}