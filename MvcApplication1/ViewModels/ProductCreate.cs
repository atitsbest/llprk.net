using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcApplication1.Models;
using System.ComponentModel.DataAnnotations;

namespace MvcApplication1.ViewModels
{
    /// <summary>
    /// ViewModel für ~/shop/index
    /// </summary>
    public class ProductCreate : ProductEdit
    {
        public Category Category { get; set; }
    }
}