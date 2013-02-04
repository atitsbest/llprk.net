using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.IO;

namespace Llprk.Web.UI.Application
{
    /// <summary>
    /// Bietet Versenden von E-Mails per SMTP an.
    /// </summary>
    public class SmtpMailProvider
    {
        /// <summary>
        /// Smtp-Server-Host.
        /// </summary>
        private readonly string _Host;

        /// <summary>
        /// Port des Smtp-Servers:
        /// </summary>
        private readonly int _Port;

        /// <summary>
        /// Authentication: Benutzername.
        /// </summary>
        private readonly string _User;

        /// <summary>
        /// Authentication: Passwort.
        /// </summary>
        private readonly string _Password;

        /// <summary>
        /// Anmeldeinfos verwenden?
        /// </summary>
        private bool _UseDefaultCredentials
        {
            get { return string.IsNullOrEmpty(_User); }
        }

        /// <summary>
        /// CTR
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        public SmtpMailProvider(string host="localhost", int port=25, string user="", string password="")
        {
            if (string.IsNullOrEmpty(host)) { throw new ArgumentNullException("host"); }

            _Host = host;
            _Port = port;
            _User = user;
            _Password = password;
        }

        /// <summary>
        /// Versendet die angegebene Mail per SMTP.
        /// </summary>
        /// <param name="message"></param>
        public void SendMail(MailMessage message)
        {
            if (message == null) { throw new ArgumentNullException("message"); }

            var client = _CreateClient();

            try {
                client.Send(message);
            }
            catch (InvalidOperationException e) {
                throw new ApplicationException("Die E-Mail konnte nicht verschickt werden.", e);
            }
            catch (SmtpFailedRecipientsException e) {
                throw new ApplicationException("Die E-Mail konnte nicht an alle Empfänger verschickt werden.", e);
            }
            catch (SmtpException e) {
                throw new ApplicationException("Die E-Mail konnte nicht verschickt werden. Der Smtp-Server hat den Fehler verursacht.", e);
            }

        }

        /// <summary>
        /// Speichert die Mail unter dem angegebenen Pfad.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="path">Wird kein Pfad angegeben, wird die Mail in einem Unterverzeichnis des temp-Dir abgelegt.</param>
        /// <returns>
        ///     Den vollen Pfad zur gespeicherten Datei, Ausnahme: Wenn <paramref name="path"/> angegeben
        ///     wurde, dann wird nur <paramref name="path"/> zurückgegeben.
        /// </returns>
        public string SaveMail(MailMessage message, string path=null)
        {
            // TODO: Fehlerbehandlung
            var client = _CreateClient();

            // Pfad bestimmen:
            var pathToUse = path; 
            if (string.IsNullOrEmpty(path)) {
                pathToUse = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                Directory.CreateDirectory(pathToUse);
            }

            // Ablegen der Mail am FS
            client.PickupDirectoryLocation = pathToUse; 
            client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
            client.Send(message);

            // Wo die Datei gespeichert wurde.
            return string.IsNullOrEmpty(path)
                ? new DirectoryInfo(pathToUse).GetFiles().First().FullName
                : path;
        }

        /// <summary>
        /// Smtp-Client erstellen und konfigurieren.
        /// </summary>
        /// <returns></returns>
        private SmtpClient _CreateClient()
        {
            var client = new SmtpClient(_Host, _Port);
            if (_UseDefaultCredentials) {
                client.UseDefaultCredentials = true;
            }
            else {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(_User, _Password);
            }
            return client;
        }

    }
}