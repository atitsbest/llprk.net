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
        /// <summary>
        /// Hash aus <CategorieName, Produkt>
        /// </summary>
        public IDictionary<string, Product[]> Categories;

        /// <summary>
        /// Liefert alle Produkte von Categories.
        /// </summary>
        public IEnumerable<dynamic> Products
        {
            get
            {
                var result = Categories.Values.SelectMany(x => x);
                return result.Select(x => new {
                                Name = x.Name,
                                Price = x.Price,
                                Id = x.Id,
                                ThumbailUrl = x.Pictures.FirstOrDefault() == null ? "": x.Pictures.First().ThumbnailUrl
                            });
            }
        }
    }
}