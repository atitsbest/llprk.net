using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Llprk.Web.UI.Models;

namespace Llprk.Web.UI.Areas.Store.Models
{
    /// <summary>
    /// ViewModel für ~/shop/index
    /// </summary>
    public class ShopLayout
    {
        public IEnumerable<Category> Categories { get; set; }
    }
}