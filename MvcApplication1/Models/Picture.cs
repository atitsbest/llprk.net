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
            get { return "http://llprk2.blob.core.windows.net/pictures/" + Id.ToString() +"_t.png"; }
        }

        public string PictureUrl
        {
            // TODO: Url konfigurierbar machen.
            get { return "http://llprk2.blob.core.windows.net/pictures/" + Id.ToString() + ".png"; }
        }

        /// <summary>
        /// Vergleich!
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return this.Id == ((Picture)obj).Id;
        }

        /// <summary>
        /// Vergleich!
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}