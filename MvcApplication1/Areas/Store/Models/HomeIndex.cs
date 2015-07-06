using Llprk.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Llprk.Web.UI.Areas.Store.Models
{
    public class HomeIndex
    {
        public IEnumerable<Product> Products { get; set; }
    }
}