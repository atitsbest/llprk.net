using Llprk.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Llprk.Web.UI.Areas.Admin.Models
{
    public class PictureDelete
    {
        public Picture Picture { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}