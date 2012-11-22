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
                return Categories.Values
                            .SelectMany(x => x)
                            .Select(x => new {
                                Name = x.Name,
                                Price = x.Price,
                                Id = x.Id,
                                ThumbailUrl = x.Pictures.First().ThumbnailUrl
                            });
            }
        }
    }
}