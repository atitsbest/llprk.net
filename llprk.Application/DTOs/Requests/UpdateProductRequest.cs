using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Llprk.Application.DTOs.Responses
{
    public class UpdateProductRequest
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }

        public int Available { get; set; }
        
        [Required]
        public int ShippingCategoryId { get; set; }

        public bool IsPublished { get; set; }
        
        public string PageTitle { get; set; }
        public string MetaDescription { get; set; }
        [Required]
        public string UrlHandle { get; set; }
    }
}
