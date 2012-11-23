using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcApplication1.Models;

namespace MvcApplication1.ViewModels
{
    /// <summary>
    /// ViewModel für ~/shop/index
    /// </summary>
    public class ShopDetail
    {
        /// <summary>
        /// Das angezeigt Produkt.
        /// </summary>
        public Product Product { get; set; }

    }
}