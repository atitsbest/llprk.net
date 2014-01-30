using Castle.Core.Logging;
using Llprk.Web.UI;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.Seleno.Configuration;

namespace llprk.Web.UI.IntegrationTests
{
    class Host
    {
          public static readonly SelenoHost Instance = new SelenoHost();

        static Host()
        {
            Instance.Run("llprk.Web.UI", 8094, config => config
                .UsingLoggerFactory(new ConsoleFactory())
                .WithRemoteWebDriver(() => BrowserFactory.InternetExplorer())
                .WithRouteConfig(RouteConfig.RegisterRoutes)
				.WithMinimumWaitTimeoutOf(TimeSpan.FromSeconds(2))
            );
        }
    }

    [SetUpFixture]
    public class HostSetup
    {
        /// <summary>
        /// Der Connection-String aus der Web.config.
        /// </summary>
        string _OriginalConnectionString;

        /// <summary>
        /// Der Pfad zur WebApp die wir testen.
        /// </summary>
        string _WebAppPath;

        /// <summary>
        /// CTR
        /// </summary>
        public HostSetup()
        {
            var cb = System.Reflection.Assembly.GetExecutingAssembly().CodeBase.Replace("file:///", "");
            _WebAppPath = Path.Combine(new DirectoryInfo(cb).Parent.Parent.Parent.Parent.FullName, "Eriksson.Web.UI");
        }

        [SetUp]
        public void RunBeforeAnyTests()
        {
            // ConnectionString aus der App.config lesen.
            var cs = ConfigurationManager.ConnectionStrings["ERIKSSONEntities"].ConnectionString;

            // Original ConnectionString speichern und neuen für die Tests setzten.
            _OriginalConnectionString = _ChangeWebConfigValuesForTest(cs);
        }

        [TearDown]
        public void RunAfterAnyTests()
        {
            _ChangeWebConfigValuesForTest(_OriginalConnectionString);
        }

        /// <summary>
        /// ConnectionString in WebConfig der WebApp ändern.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="connectionString"></param>
        /// <returns>Alten ConnectionString</returns>
        string _ChangeWebConfigValuesForTest(string connectionString)
        {
            // Get the configuration file.
            var map = new ExeConfigurationFileMap();
            map.ExeConfigFilename = Path.Combine(_WebAppPath, "web.config");

            // TFS: Zur Sicherheit den Schreibschutz der web.config entfernen.
            File.SetAttributes(map.ExeConfigFilename, File.GetAttributes(map.ExeConfigFilename) & ~FileAttributes.ReadOnly);

            var webConfig = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);

            // Change connection string setting
            var connectionStringSettings = webConfig.ConnectionStrings.ConnectionStrings["ERIKSSONEntities"];
            var old = connectionStringSettings.ConnectionString;
            connectionStringSettings.ConnectionString = connectionString;

            // Save the config file 
            webConfig.Save();

            return old;
        }
    }
}
