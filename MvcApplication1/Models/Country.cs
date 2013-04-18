using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Llprk.Web.UI.Models
{
    public class Country
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ShippingCost> ShippingCosts { get; set; }

		/// <summary>
		/// Wie hoch sind die Versandkosten für die angegebene 
        /// Kategorie in dieses Land?
		/// <returns>Die Versandkosten. 0.0 wenn keine Versandkosten hinterlegt wurden.</returns>
		/// </summary>
        public decimal ShippingCost(ShippingCategory category)
        {
            return (from sc in ShippingCosts
                    where sc.ShippingCategoryId == category.Id
                    select sc.Amount).FirstOrDefault();
        }

        /// <summary>
        /// Vergleich!
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return this.Id == ((Country)obj).Id;
        }

        /// <summary>
        /// Vergleich!
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}