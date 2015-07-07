using DotLiquid;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Llprk.Web.UI.Liquid
{
    public static class ScriptTagFilter
    {
        public static string ScriptTag(string input)
        {
            var url = input;
            // TODO: installierte Theme verwenden.
            if (!Uri.IsWellFormedUriString(input, UriKind.Absolute))
            {
                UrlHelper helper = new UrlHelper(HttpContext.Current.Request.RequestContext);
                url = helper.Content("~/Themes/minimal/assets/scripts/" + input);
            }
            return string.Format("<script type=\"text/javascript\" src=\"{0}\"></script>", url);
        }
    }

}