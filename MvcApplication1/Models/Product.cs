﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcApplication1.Models
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
        
        public ICollection<Picture> Pictures { get; set; }

        public ICollection<OrderLine> OrderLines { get; set; }

        public Product()
        {
            Pictures = new HashSet<Picture>();
            Available = 1;
        }
    }
}