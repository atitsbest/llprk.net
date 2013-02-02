using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Llprk.Web.UI.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]    
        public decimal Price { get; set; }
        
        public int CategoryId { get; set; }
        [Display(Name="Category")]
        public Category Category { get; set; }

        [Display(Name="published?")]
        public bool IsPublished { get; set; }

        /// <summary>
        /// Wieviele davon sind verfügbar.
        /// </summary>
        public int Available { get; set; }

        public virtual ICollection<Product_Picture> Pictures { get; set; }

        public ICollection<Tag> Tags { get; set; }

        /// <summary>
        /// Alle Bilder in der richtigen Reihenfolge.
        /// </summary>
        public IEnumerable<Picture> OrderedPictures {
            get
            {
                return Pictures
                    .OrderBy(p => p.Pos)
                    .Select(p => p.Picture);
            }
        } 

        /// <summary>
        /// Liefert das erste Bild, wenn es eines Gibt.
        /// </summary>
        public Picture FirstPicture
        {
            get { return OrderedPictures.FirstOrDefault(); }
        }


        /// <summary>
        /// CTR
        /// </summary>
        public Product()
        {
            Available = 1;
            Pictures = new HashSet<Product_Picture>();
            Tags = new HashSet<Tag>();
        }

        /// <summary>
        /// Vergleich!
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return this.Id == ((Product)obj).Id;
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