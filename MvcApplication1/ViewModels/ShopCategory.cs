﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Llprk.Web.UI.Models;

namespace Llprk.Web.UI.ViewModels
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