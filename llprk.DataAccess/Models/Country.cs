using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Llprk.DataAccess.Models
{
    public class Country
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ShippingCost> ShippingCosts { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Tax> Taxes { get; set; }

		/// <summary>
		/// Wie hoch sind die Versandkosten für die angegebene 
        /// Kategorie in dieses Land?
		/// <returns>Die Versandkosten. 0.0 wenn keine Versandkosten hinterlegt wurden.</returns>
		/// </summary>
        public decimal ShippingCost(ShippingCategory category)
        {
            if (category == null) { throw new ArgumentNullException("Keine Versandkosten ohne Kategorie möglich!"); }
            return (from sc in ShippingCosts
                    where sc.ShippingCategoryId == category.Id
                    select sc.Amount).FirstOrDefault();
        }

        /// <summary>
        /// Vergleich!
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return string.Format("Country_{0}", Id).GetHashCode();
        }
    }
}