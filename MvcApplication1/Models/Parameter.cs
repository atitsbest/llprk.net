using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Llprk.Web.UI.Models
{
    public class Parameter
    {
        public int Id { get; set; }
        [DisplayName("Mail sent, when the order has been placed.")]
        public string MailMessageOrdered { get; set; }
        [DisplayName("Mail sent, when the invoice has been paid.")]
        public string MailMessagePaid { get; set; }
        [DisplayName("Mail sent, when the order has been shipped.")]
        public string MailMessageShipped { get; set; }
    }
}