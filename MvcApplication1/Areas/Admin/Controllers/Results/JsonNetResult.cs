using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Llprk.Web.UI.Controllers.Results
{
    public class JsonNetResult : JsonResult
    {
        public new Encoding ContentEncoding { get; set; }
        public new string ContentType { get; set; }
        public new object Data { get; set; }

        public JsonSerializerSettings SerializerSettings { get; set; }
        public Formatting Formatting { get; set; }

        /// <summary>
        /// CTR
        /// </summary>
        /// <param name="data"></param>
        public JsonNetResult(object data)
        {
            Data = data;
            SerializerSettings = new JsonSerializerSettings { 
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
            };
        }

        /// <summary>
        /// Executes the Result.
        /// </summary>
        /// <param name="context"></param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            HttpResponseBase response = context.HttpContext.Response;

            response.ContentType = !string.IsNullOrEmpty(ContentType)
              ? ContentType
              : "application/json";

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            if (Data != null)
            {
                JsonTextWriter writer = new JsonTextWriter(response.Output) { Formatting = Formatting };

                JsonSerializer serializer = JsonSerializer.Create(SerializerSettings);
                serializer.Serialize(writer, Data);

                writer.Flush();
            }
        }
    }
}