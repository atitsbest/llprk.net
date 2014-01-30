using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace llprk.Web.UI.IntegrationTests
{
    /// <summary>
    /// Liefert die Basis für einen Integrationstest mit Selenium.
    /// </summary>
    [Category("Integration Test")]
    public abstract class IntegrationTest
    {
        /// <summary>
        /// Selenium Driver
        /// </summary>
        protected readonly IWebDriver Driver = Host.Instance.Application.Browser;

        /// <summary>
        /// Basis-Url für die App.
        /// </summary>
        protected readonly Uri BaseUrl = new Uri(Host.Instance.Application.WebServer.BaseUrl);

        [SetUp]
        public virtual void BeforeEachTest()
        {
            var connectionString = @"";
            var queryString = @"
DELETE FROM Project;
DELETE FROM Customer;
";
            using (var connection = new SqlConnection(connectionString)) {
                var command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Anmelden, wenn das verlangt wird.
        /// </summary>
        protected void LoginIfNeccessary()
        {
            //if (Driver.Url.ToLower().Contains("/login")) {
            //    new LoginPage(Driver).Login("admin", "Pin.123");
            //}
        }

        /// <summary>
        /// Überprüft, ob ein Fehler passiert ist. Dazu dient die Verwendung von Toastr.
        /// </summary>
        /// <returns></returns>
        protected bool HasError()
        {
            return Driver.PageSource.Contains("toastr.error(");
        }
    }
}
