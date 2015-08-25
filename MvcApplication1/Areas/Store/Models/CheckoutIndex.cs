using Llprk.Application.DTOs.Requests;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Llprk.Web.UI.Areas.Store.Models
{
    public class CheckoutIndex
    {
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

        public CreateOrderRequest.Address DeliveryAddress { get; set; }
        public CreateOrderRequest.Address BillingAddress { get; set; }
        public LineItem[] LineItems { get; set; }
        public Country[] Countries { get; set; }

        public string SubTotal { get; set; }
        public string Tax { get; set; }
        public string ShippingCosts { get; set; }
        public string Total { get; set; }

        public string CheckoutShippingCostsUrl { get; set; }


        /// <summary>
        /// CTR
        /// </summary>
        public CheckoutIndex()
        {
            LineItems = new LineItem[] { };
            Countries = new Country[] { };
            DeliveryAddress = new CreateOrderRequest.Address();
            var url = new UrlHelper(HttpContext.Current.Request.RequestContext);
            CheckoutShippingCostsUrl = url.Action(MVC.Store.Checkout.VariableCosts());
        }
    }
}