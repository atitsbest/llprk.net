using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Llprk.Web.UI.Models
{
    public class Product_Picture
    {
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public Guid PictureId { get; set; }
        public virtual Picture Picture { get; set; }
        public int Pos { get; set; }
    }
}