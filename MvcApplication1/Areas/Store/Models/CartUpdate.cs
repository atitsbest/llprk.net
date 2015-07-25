using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Llprk.DataAccess.Models;
using DotLiquid;
using Llprk.Web.UI.ViewModels;
using System.Web.Mvc;
using Llprk.Application.DTOs.Requests;

namespace Llprk.Web.UI.Areas.Store.Models
{
    /// <summary>
    /// ViewModel für ~/shop/index
    /// </summary>
    public class CartUpdate
    {
        public class LineItemUpdate
        {
            public int Id { get; set; }
            public int Qty { get; set; }
        }

        public UpdateLineItemQtyRequest[] Updates { get; set; }
    }
}