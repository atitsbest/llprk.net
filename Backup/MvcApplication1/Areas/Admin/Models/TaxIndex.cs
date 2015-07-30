using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Llprk.Web.UI.Areas.Admin.Models
{
    public class TaxIndex
    {
        public class Country
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public int TaxId { get; set; }
            public int TaxPercent { get; set; }
        }

        public Country[] Countries { get; set; }
    }
}