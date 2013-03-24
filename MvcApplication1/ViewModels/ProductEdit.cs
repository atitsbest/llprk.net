using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Llprk.Web.UI.Models;
using System.ComponentModel.DataAnnotations;

namespace Llprk.Web.UI.ViewModels
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

        [Required]
        public int Available { get; set; }
        
        public int CategoryId { get; set; }

        [Display(Name="published?")]
        public bool IsPublished { get; set; }
        
        public ICollection<Picture> Pictures { get; set; }

        public IEnumerable<Picture> AllPictures { get; set; }

        public IEnumerable<Guid> AssignedPictureIds
        {
            get
            {
                return Pictures == null
                    ? new Guid[] { }
                    : Pictures.Select(p => p.Id);
            }
        }

        public ICollection<Tag> Tags { get; set; }
        public IEnumerable<Tag> AllTags { get; set; }

        /// <summary>
        /// CTR
        /// </summary>
        public ProductEdit()
        {
            Available = 1;
            Tags = new HashSet<Tag>();
        }
    }
}