using Llprk.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Llprk.Web.UI.ViewModels
{
    public class HomeIndex
    {
        public IEnumerable<Product> Products { get; set; }
    }
}