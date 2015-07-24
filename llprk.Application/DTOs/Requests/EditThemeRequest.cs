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
        public class Item
        {
            public string Name { get; private set; }
            public string MimeType { get; private set; }
            public string Type { get; private set; } // layout, asset, template, snippet.

            public Item(string name, string type)
            {
                if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("name");
                if (string.IsNullOrWhiteSpace(type)) throw new ArgumentNullException("type");

                Name = Path.GetFileName(name);
                Type = type;
                MimeType = MimeTypes.MimeTypeMap.GetMimeType(Path.GetExtension(name));
            }
        }

        /// <summary>
        /// Name of the theme.
        /// </summary>
        public string Name { get; set; }

        public Dictionary<string, Item[]> Items { get; set; }

        public EditThemeRequest(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("name");

            Name = name;
        }
    }
}
