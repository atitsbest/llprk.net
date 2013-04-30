using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Llprk.Web.UI.Models
{
    public class ShippingCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }

		[JsonIgnore]
        public virtual ICollection<ShippingCost> ShippingCosts { get; set; }
		[JsonIgnore]
        public virtual ICollection<Product> Products { get; set; }
    }
}