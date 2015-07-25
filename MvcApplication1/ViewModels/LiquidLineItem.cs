using DotLiquid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Llprk.Web.UI.ViewModels
{
    public class LiquidLineItem : Drop
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public DateTime CreatedAt { get; set; }
        public LiquidProduct Product { get; set; }

    }
}