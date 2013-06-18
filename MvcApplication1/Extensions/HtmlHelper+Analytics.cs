using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace Llprk.Web.UI.Extensions
{
    public static partial class HtmlHelper_Analytics
    {
        /// <summary>
        /// Insert GA with an Urchin and domain name.
        /// Wird NICHT in DEBUG gesetzt.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="urchin"></param>
        /// <param name="domainName"></param>
        /// <returns></returns>
        public static HtmlString Analytics(this HtmlHelper htmlHelper, string urchin, string domainName)
        {
            var sb = new StringBuilder();

			var script = string.Format(@"(function(i,s,o,g,r,a,m){{i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){{
	(i[r].q=i[r].q||[]).push(arguments)}},i[r].l=1*new Date();a=s.createElement(o),
	m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
}})(window,document,'script','//www.google-analytics.com/analytics.js','ga');

ga('create', '{0}', '{1}');
ga('send', 'pageview');", urchin, domainName);

            sb.Append("<script type='text/javascript'>");
#if !DEBUG
            sb.Append(script);
#endif
            sb.Append("</script>");

            return new HtmlString(sb.ToString());
        }

        /// <summary>
        /// Pull the urchin and domain name from Web.Config
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static HtmlString Analytics(this HtmlHelper htmlHelper)
        {
            //pull values from Config
            string urchin = ConfigurationManager.AppSettings["ga-urchin"];
            string domainName = ConfigurationManager.AppSettings["ga-domainName"];
            return Analytics(htmlHelper, urchin, domainName);
        }
    }
}