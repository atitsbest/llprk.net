using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Llprk.DataAccess.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }

        public int? OrderId { get; set; }

        public virtual ICollection<LineItem> LineItems { get; set; }

        public virtual Order Order { get; set; }

        public decimal Subtotal {
            get
            {
                return LineItems != null
                    ? LineItems.Sum(l => l.Subtotal)
                    : 0;
            }
        }

        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return this.Id == ((Cart)obj).Id;
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