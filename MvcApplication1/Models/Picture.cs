using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Llprk.Web.UI.Models
{
    public class Picture
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

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
    }
}