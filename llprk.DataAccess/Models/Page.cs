using System;
using System.Linq;
using System.Collections.Generic;

namespace Llprk.DataAccess.Models
{
    public class Page
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UrlHandle { get; set; }
        public bool IsPublished { get; set; }

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