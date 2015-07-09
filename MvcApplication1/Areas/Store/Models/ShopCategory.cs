using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Llprk.DataAccess.Models;

namespace Llprk.Web.UI.Areas.Store.Models
{
    /// <summary>
    /// ViewModel für ~/shop/index
    /// </summary>
    public class ShopCategory
    {
        public Category Category;

        public IEnumerable<Product> Products;
    }
}