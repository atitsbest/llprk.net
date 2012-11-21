using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcApplication1.Models;
using System.ComponentModel.DataAnnotations;

namespace MvcApplication1.ViewModels
{
    /// <summary>
    /// ViewModel für ~/shop/index
    /// </summary>
    public class ProductEdit
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]    
        public decimal Price { get; set; }
        
        public int CategoryId { get; set; }

        [Display(Name="published?")]
        public bool IsPublished { get; set; }
        
        public ICollection<Picture> Pictures { get; set; }

        public IEnumerable<Picture> AllPictures { get; set; }
    }
}