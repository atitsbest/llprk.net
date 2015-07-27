using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Llprk.Web.UI.Areas.Store.Models
{
    public class CheckoutIndex
    {
        public class Address
        {
            public string Anrede { get; set; }
            public string Vorname { get; set; }
            public string Nachname { get; set; }
            public string Adresszusatz { get; set; }
            public string StrasseNr { get; set; }
            public string Plz { get; set; }
            public string Ort { get; set; }
            public int LandId { get; set; }
            public string Email { get; set; }
        }

        public class LineItem 
        {
            public string Name { get; set; }
            public int Qty { get; set; }
            public decimal Price { get; set; }
            public decimal Subtotal { get { return Qty * Price; } }
        }

        public class Country
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

        public Address Lieferadresse { get; set; }
        public Address Rechnungsadresse { get; set; }
        public bool RechnungsLieferungsAdresseGleich { get; set; }
        public LineItem[] LineItems { get; set; }
        public Country[] Countries { get; set; }

        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal ShippingCosts { get; set; }
        public decimal Total
        {
            get
            {
                return SubTotal
                    + Tax
                    + ShippingCosts;
            }
        }

        /// <summary>
        /// CTR
        /// </summary>
        public CheckoutIndex()
        {
            LineItems = new LineItem[] { };
        }
    }
}