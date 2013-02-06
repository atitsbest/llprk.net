using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Llprk.Web.UI.Models;

namespace Llprk.Web.UI.ViewModels
{
    /// <summary>
    /// ViewModel für ~/shop/index
    /// </summary>
    public class ShopIndex
    {
        public IEnumerable<ShopCategory> Categories;

        /// <summary>
        /// Liefert alle Produkte von Categories.
        /// </summary>
        public IEnumerable<Product> Products
        {
            get
            {
                return Categories.SelectMany(x => x.Products);
            }
        }
    }
}