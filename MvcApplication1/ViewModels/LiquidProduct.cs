using DotLiquid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Llprk.Web.UI.ViewModels
{
    public class LiquidProduct : Drop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsPublished { get; set; }
        public int Available { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}