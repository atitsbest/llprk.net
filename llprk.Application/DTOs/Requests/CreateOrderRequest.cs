using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Llprk.Application.DTOs.Requests
{
    public class CreateOrderRequest
    {
        public class Address
        {
            [Required]
            public string Salutation { get; set; }
            [Required]
            public string Firstname { get; set; }
            [Required]
            public string Lastname { get; set; }
            public string Address1 { get; set; }
            [Required]
            public string Address2 { get; set; }
            [Required]
            public string Zip { get; set; }
            [Required]
            public string City { get; set; }
            [Required]
            public string CountryId { get; set; }
            [Required, EmailAddress]
            public string Email { get; set; }
        }

        [Required]
        public Address DeliveryAddress { get; set; }
        public Address BillingAddress { get; set; }
    }
}
