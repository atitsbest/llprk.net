using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Llprk.DataAccess.Models
{
    public class Tax
    {
        public int Id { get; set; }
        public int Percent { get; set; }
        public string CountryId { get; set; }

        public Country Country { get; set; }

        /// <summary>
        /// Vergleich!
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return string.Format("Tax_{0}", Id).GetHashCode();
        }
    }
}