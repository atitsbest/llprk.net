using Llprk.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Llprk.Web.UI.ViewModels
{
    public class ParameterIndex : Parameter
    {
	[Remote("IsMailTemplateValid", "Parameters", HttpMethod="POST")]
        public string MailMessageOrdered { get; set; }
	[Remote("IsMailTemplateValid", "Parameters", HttpMethod="POST")]
        public string MailMessagePaid { get; set; }
	[Remote("IsMailTemplateValid", "Parameters", HttpMethod="POST")]
        public string MailMessageShipped { get; set; }
    }
}