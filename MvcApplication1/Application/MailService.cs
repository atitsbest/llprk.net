using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace Llprk.Web.UI.Application
{
    public class MailService
    {
        /// <summary>
        /// Schickt eine HTML-Mail an die Email-Adresse aus dem Auftrag.
        /// </summary>
        /// <param name="order"></param>
        /// <param name="subject"></param>
        /// <param name="mailBody"></param>
        public static void SendMailToCustomer(string emailAddress, string subject, string mailBody)
        {
			if (string.IsNullOrWhiteSpace(emailAddress)) { throw new ArgumentException("Keine E-Mail-Adresse angegeben!"); }

            var smtpServer = ConfigurationManager.AppSettings["SMTPServer"];
            var smtpUser = ConfigurationManager.AppSettings["SMTPUser"];
            var smtpPwd = ConfigurationManager.AppSettings["SMTPPwd"];
            var senderAddress = ConfigurationManager.AppSettings["EmailSenderAddress"];

            var message = new MailMessage(senderAddress, emailAddress, subject, mailBody);
            message.IsBodyHtml = true;
            message.BodyEncoding = Encoding.UTF8;

            var mp = new SmtpMailProvider(
                smtpServer,
                user: smtpUser,
                password: smtpPwd,
                enableSSL: true);
            mp.SendMail(message);
        }

        /// <summary>
        /// Schickt eine HTML-Mail an den Eigentümer des WebShops.
        /// </summary>
        /// <param name="order"></param>
        /// <param name="subject"></param>
        /// <param name="mailBody"></param>
        public static void SendMailToOwner(string subject, string mailBody)
        {
            var ownerAddress = ConfigurationManager.AppSettings["EmailSenderAddress"];
            SendMailToCustomer(ownerAddress, subject, mailBody);
        }
    }
}