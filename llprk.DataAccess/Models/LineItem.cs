using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Llprk.DataAccess.Models
{
    public class LineItem
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public DateTime CreatedAt { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int? CartId { get; set; }
        public Cart Cart { get; set; }
        public int? OrderId { get; set; }
        public Order Order { get; set; }

        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return this.Id == ((LineItem)obj).Id;
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