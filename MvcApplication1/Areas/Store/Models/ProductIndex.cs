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
    public class ProductIndex : Drop
    {
        /// <summary>
        /// Liefert alle Produkte von Categories.
        /// </summary>
        public LiquidProduct Product { get; set; }

        /// <summary>
        /// Url to post product to cart.
        /// </summary>
        public string AddToCartUrl
        {
            get
            {
                var helper = new UrlHelper(HttpContext.Current.Request.RequestContext);
                return helper.Action(MVC.Store.Cart.Add(Product.Id, 1));
            }
        }
        public string page_title { get { return "Product"; } }
        public string template { get { return "product"; } }

    }
}