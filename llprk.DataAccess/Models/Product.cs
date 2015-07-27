using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Llprk.DataAccess.Models
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

        public int ShippingCategoryId { get; set; }
        public ShippingCategory ShippingCategory { get; set; }

        [Display(Name="published?")]
        public bool IsPublished { get; set; }

        public string PageTitle { get; set; }
        public string MetaDescription { get; set; }
        public string UrlHandle { get; set; }


        /// <summary>
        /// Wieviele davon sind verfügbar.
        /// </summary>
        public int Available { get; set; }

        public virtual ICollection<Picture> Pictures { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }

        public virtual ICollection<OrderLine> OrderLines { get; set; }

        public virtual ICollection<LineItem> LineItems { get; set; }

        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Alle Bilder in der richtigen Reihenfolge.
        /// </summary>
        public IEnumerable<Picture> OrderedPictures {
            get
            {
                return Pictures
                    .OrderBy(p => p.Pos);
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
		/// Liefert den ersten Satz der Beschreibung.
		/// </summary>
        public string ShortDescription
        {
            get {
                var result = Regex.Replace((Description ?? ""), "<.*?>", string.Empty); ;
                string[] sentences = Regex.Split(result, @"(?<=[\.!\?])\s?");
                return sentences != null && sentences.Length > 0
                    ? sentences[0]
                    : "";
            }
        }

		/// <summary>
		/// Ist das ein neues Produkt? Ja, wenn vor weniger als zwei Wochen erstellt.
		/// </summary>
        public bool IsNew {
            get {
                var diff = DateTime.Now - CreatedAt;
                return diff.Days < 14;
            }
        }


        /// <summary>
        /// CTR
        /// </summary>
        public Product()
        {
            Available = 1;
            Pictures = new HashSet<Picture>();
            Tags = new HashSet<Tag>();
        }

        /// <summary>
        /// Vergleich!
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return string.Format("Product_{0}", Id).GetHashCode();
        }
    }
}