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
    public class ShopDetail
    {
        /// <summary>
        /// Das angezeigt Produkt.
        /// </summary>
        public Product Product { get; set; }

        public dynamic SerializeableProduct
        {
            get
            {
                return new {
                    Name = Product.Name,
                    Price = Product.Price,
                    Id = Product.Id,
					ShippingCategoryId = Product.ShippingCategoryId,
                    ThumbailUrl = Product.FirstPicture == null ? "" : Product.FirstPicture.ThumbnailUrl
                };
            }
        }

    }
}