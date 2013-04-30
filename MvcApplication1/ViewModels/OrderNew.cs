using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Llprk.Web.UI.Models;
using System.ComponentModel.DataAnnotations;

namespace Llprk.Web.UI.ViewModels
{
    /// <summary>
    /// ViewModel für ~/orders/new
    /// </summary>
    // TODO: Validierung von "Order" übernehmen.
    public class OrderNew
    {
        public class ProductLine {
            public int Id { get; set; }
            public int Qty { get; set; }
        }

        public string Salutation { get; set; }
        public string Firstname { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string CountryCode { get; set; }
        public string Email { get; set; }
        public string Comment { get; set; }
        public string PaymentType { get; set; }
        public IEnumerable<ProductLine> Products { get; set; }

        public bool UsesPayPal {
            get { return (PaymentType ?? "").ToUpper() == "PAYPAL"; }
        }
        public bool UsesTransaction {
            get { return (PaymentType ?? "").ToUpper() == "WIRE"; }
        }
    }
}