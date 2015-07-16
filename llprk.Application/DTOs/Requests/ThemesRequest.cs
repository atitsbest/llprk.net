using Llprk.Application.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Llprk.Application.DTOs.Requests
{
    public class ThemesRequest
    {
        public class Theme
        {
            public string Name { get; set; }
            public bool IsActive { get; set; }
        }

        public IEnumerable<Theme> Themes { get; set; }
    }
}
