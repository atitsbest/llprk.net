using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Llprk.Web.UI.Models;
using System.ComponentModel.DataAnnotations;

namespace Llprk.Web.UI.ViewModels
{
    /// <summary>
    /// ViewModel für ~/shop/index
    /// </summary>
    public class ProductCreate : ProductEdit
    {
        public Category Category { get; set; }
    }
}