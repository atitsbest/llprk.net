using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Llprk.DataAccess.Models
{
    public class ShippingCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ShippingCost> ShippingCosts { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}