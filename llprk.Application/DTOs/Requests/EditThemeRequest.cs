using Llprk.DataAccess.Models.Theme;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Llprk.Application.DTOs.Requests
{
    public class EditThemeRequest
    {
        /// <summary>
        /// Name of the theme.
        /// </summary>
        public string Name { get; set; }

        public Dictionary<string, IUnpublishedThemeItem[]> Items { get; set; }

        public EditThemeRequest(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("name");

            Name = name;
        }
    }
}
