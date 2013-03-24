using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Llprk.Web.UI.Models
{
    public class Parameter
    {
        public int Id { get; set; }
        [DisplayName("Mail sent, when the order has been placed."), AllowHtml]
        public string MailMessageOrdered { get; set; }
        [DisplayName("Mail sent, when the invoice has been paid."), AllowHtml]
        public string MailMessagePaid { get; set; }
        [DisplayName("Mail sent, when the order has been shipped."), AllowHtml]
        public string MailMessageShipped { get; set; }
    }
}