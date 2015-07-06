using Llprk.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Llprk.Web.UI.Areas.Admin.Models
{
    public class ParameterIndex : Parameter
    {
        [Remote("IsMailTemplateValid", "Parameters", HttpMethod = "POST")]
        public new string MailMessageOrdered { get; set; }
        [Remote("IsMailTemplateValid", "Parameters", HttpMethod = "POST")]
        public new string MailMessagePaid { get; set; }
        [Remote("IsMailTemplateValid", "Parameters", HttpMethod = "POST")]
        public new string MailMessageShipped { get; set; }
    }
}