﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcApplication1.Models;
using System.ComponentModel.DataAnnotations;

namespace MvcApplication1.ViewModels
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
        public string Email { get; set; }
        public IEnumerable<ProductLine> Products { get; set; }
    }
}