using Llprk.DataAccess.Models;
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

        public Address Lieferadresse { get; set; }
        public Address Rechnungsadresse { get; set; }
        public bool RechnungsLieferungsAdresseGleich { get; set; }
        public LineItem[] LineItems { get; set; }
    }
}