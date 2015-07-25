using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Llprk.DataAccess.Models;
using DotLiquid;
using Llprk.Web.UI.ViewModels;
using System.Web.Mvc;

namespace Llprk.Web.UI.Areas.Store.Models
{
    /// <summary>
    /// ViewModel für ~/shop/index
    /// </summary>
    public class CartIndex : Drop
    {
        public IEnumerable<LiquidLineItem> LineItems { get; set; }
    }
}