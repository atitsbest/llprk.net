using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Llprk.DataAccess.Models;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Llprk.Web.UI.Areas.Admin.Models
{
    /// <summary>
    /// ViewModel für ~/orders/new
    /// </summary>
    // TODO: Validierung von "Order" übernehmen.
    public class PostOrderNew
    {
        public class ProductLine {
            public int Id { get; set; }
            public int Qty { get; set; }
        }

        [Required, Display(Name="Anrede")] 
        public string Salutation { get; set; }

		[Required, MinLength(2), MaxLength(30), Display(Name="Vorname")] 
        public string Firstname { get; set; }

        [Required, MinLength(2), MaxLength(30), Display(Name="Name")]
        public string Name { get; set; }

        [MaxLength(30), Display(Name="Adresszusatz / Firma")]
        public string Address1 { get; set; }

        [Required, MaxLength(30), Display(Name="Strasse, Nr.")] 
        public string Address2 { get; set; }

        [Required, MaxLength(5), MinLength(4), Display(Name="Plz")] 
        public string Zip { get; set; }

        [Required, MaxLength(30), Display(Name="Ort")] 
        public string City { get; set; }

        [Required, RegularExpression("^[a-z]{2}$")]
        public string CountryCode { get; set; }

        [Required, EmailAddress, Display(Name="E-Mail")] 
        public string Email { get; set; }

        [MaxLength(1024)]public string Comment { get; set; }
        [Required, RegularExpression("^(PAYPAL|WIRE)$")] public string PaymentType { get; set; }
        [Required, MinLength(1)] public ProductLine[] Products { get; set; }

        public bool UsesPayPal {
            get { return (PaymentType ?? "").ToUpper() == "PAYPAL"; }
        }
        public bool UsesTransaction {
            get { return (PaymentType ?? "").ToUpper() == "WIRE"; }
        }
    }
}