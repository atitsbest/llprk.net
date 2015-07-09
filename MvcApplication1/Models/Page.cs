using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Llprk.Web.UI.Models
{
    public class Page
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UrlHandle { get; set; }

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