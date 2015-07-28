using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Llprk.Web.UI.Areas.Admin.Controllers.Helpers
{
    public class RenderJsonNetHelper
    {
        public static IHtmlString RenderJsonNet(object model)
        {
            return new HtmlString(JsonConvert.SerializeObject(model, System.Web.Http.GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings));
        }
    }
}