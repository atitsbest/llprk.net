using DotLiquid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Llprk.Web.UI.ViewModels
{
    public class LiquidProductPicture : Drop
    {
        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }
    }
}