using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Llprk.Web.UI.Areas.Admin.Models
{
    public class ThemeContent
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Theme { get; set; }
        public string Content { get; set; }
        public string ContentType { get; set; }

        public string UID
        {
            get
            {
                return string.Format("{0}_{1}",
                    Type,
                    Id.Replace('.', '_'));
            }
        }

        public string EditorMode
        {
            get
            {
                if (ContentType == "application/x-liquid-template")
                    return "htmlmixedliquid";
                return ContentType;
            }
        }
    }
}