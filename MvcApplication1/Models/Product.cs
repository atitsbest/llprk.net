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

        public int Available { get; set; }

        public Guid? Picture1Id { get; set; }
        public Picture Picture1 { get; set; }
        public Guid? Picture2Id { get; set; }
        public Picture Picture2 { get; set; }
        public Guid? Picture3Id { get; set; }
        public Picture Picture3 { get; set; }
        public Guid? Picture4Id { get; set; }
        public Picture Picture4 { get; set; }
        public Guid? Picture5Id { get; set; }
        public Picture Picture5 { get; set; }


        public ICollection<Tag> Tags { get; set; }

        public ICollection<OrderLine> OrderLines { get; set; }

        /// <summary>
        /// Liste mit allen verfügbaren Bilden. Sind nur 3 Bilder vorhanden,
        /// ist die Liste auch nur 3 Einträge lang.
        /// </summary>
        public IEnumerable<Picture> Pictures {
            get {
                var result = new List<Picture>();
                if (Picture1 != null) { result.Add(Picture1); }
                if (Picture2 != null) { result.Add(Picture2); }
                if (Picture3 != null) { result.Add(Picture3); }
                if (Picture4 != null) { result.Add(Picture4); }
                if (Picture5 != null) { result.Add(Picture5); }
                return result.ToArray();
            }
        }

        /// <summary>
        /// CTR
        /// </summary>
        public Product()
        {
            Available = 1;
        }
    }
}