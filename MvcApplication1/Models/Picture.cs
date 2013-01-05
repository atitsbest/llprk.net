using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace MvcApplication1.Models
{
    public class Picture
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        [ScriptIgnore]
        public ICollection<Product> Products { get; set; }

        public string ThumbnailUrl
        {
            // TODO: Url konfigurierbar machen.
            get { return "http://llprk.blob.core.windows.net/pictures/" + Id.ToString() +"_t.png"; }
        }

        public string PictureUrl
        {
            // TODO: Url konfigurierbar machen.
            get { return "http://llprk.blob.core.windows.net/pictures/" + Id.ToString() + ".png"; }
        }

        public Picture()
        {
            Products = new HashSet<Product>();
        }
    }
}