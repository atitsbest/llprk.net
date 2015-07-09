using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Llprk.DataAccess.Models;
using DotLiquid;
using Llprk.Web.UI.ViewModels;

namespace Llprk.Web.UI.Areas.Store.Models
{
    /// <summary>
    /// ViewModel für ~/shop/index
    /// </summary>
    public class ShopIndex : Drop
    {
        public IEnumerable<Category> Categories { get; set; }

        /// <summary>
        /// Liefert alle Produkte von Categories.
        /// </summary>
        public IEnumerable<LiquidProduct> Products { get; set; }

		/// <summary>
		/// Liste mit allen Banners die angezeigt werden sollen.
		/// </summary>
        public IEnumerable<string> BannerUrls { get; set; }
    }
}