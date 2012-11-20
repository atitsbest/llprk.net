using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Models
{
    public class Picture
    {
        public Guid Id { get; set; }
        public string Test { get; set; }

        public ICollection<Product> Products { get; set; }

        public string ThumbnailUrl
        {
            get { return "http://llprk.blob.core.windows.net/pictures/" + Id.ToString() +"_t.png"; }
        }

        public string PictureUrl
        {
            get { return "http://llprk.blob.core.windows.net/pictures/" + Id.ToString() + ".png"; }
        }

        public Picture()
        {
            Products = new HashSet<Product>();
        }
    }
}