using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Llprk.Web.UI.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string Salutation { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Name { get; set; }
        public string Address1 { get; set; }
        [Required]
        public string Address2 { get; set; }
        [Required, MinLength(4)]
        public string Zip { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string CountryId { get; set; }
        public virtual Country Country { get; set; }
        [EmailAddress]
        public string Email { get; set; }


        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? PaidAt { get; set; }
        public DateTime? ShippedAt { get; set; }

        [MaxLength(1024)]
        public string Comment { get; set; }

        public virtual ICollection<OrderLine> OrderLines { get; set; }

        /// <summary>
        /// Preis OHNE Versandkosten.
        /// </summary>
        public decimal TotalPrice {
            get {
                return OrderLines
                    .Select(ol => ol.Product.Price * ol.Qty)
                    .Sum();
            }
        }

        /// <summary>
        /// Wurde schon gezahlt?
        /// </summary>
        public bool IsPaid { get { return PaidAt != null; } }

        /// <summary>
        /// Wurde der Auftrag schon verschickt?
        /// </summary>
        public bool IsShipped { get { return ShippedAt != null; } }

        /// <summary>
        /// CTR
        /// </summary>
        public Order()
        {
            OrderLines = new HashSet<OrderLine>();
        }
    }
}