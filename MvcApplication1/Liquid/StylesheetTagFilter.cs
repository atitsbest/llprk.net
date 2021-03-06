﻿using DotLiquid;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Llprk.Web.UI.Liquid
{
    public static class StylesheetTagFilter
    {
        public static string StylesheetTag(string input)
        {
            var url = input;
            // TODO: installierte Theme verwenden.
            if (!Uri.IsWellFormedUriString(input, UriKind.Absolute))
            {
                UrlHelper helper = new UrlHelper(HttpContext.Current.Request.RequestContext);
                url = helper.Content(Path.Combine("~/Themes/minimal/unpublished/assets/", input));
            }

            return string.Format("<link media=\"all\" href=\"{0}\" rel=\"stylesheet\" />", url);
        }
    }

}