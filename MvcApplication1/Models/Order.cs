using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcApplication1.Models
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
        public string CountryCode { get; set; }
        [EmailAddress]
        public string Email { get; set; }

        public virtual ICollection<OrderLine> OrderLines { get; set; }

        /// <summary>
        /// CTR
        /// </summary>
        public Order()
        {
            OrderLines = new HashSet<OrderLine>();
        }
    }
}