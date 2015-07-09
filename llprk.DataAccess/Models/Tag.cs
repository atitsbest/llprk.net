using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Llprk.DataAccess.Models
{
    public class Tag
    {
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        /// <summary>
        /// Vergleich!
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return this.Id == ((Tag)obj).Id;
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