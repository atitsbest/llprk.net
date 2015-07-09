using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Llprk.Application.DTOs.Responses
{
    public class NewPageResponse
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public bool IsPublished { get; set; }
        [Required]
        public string UrlHandle { get; set; }
    }
}
