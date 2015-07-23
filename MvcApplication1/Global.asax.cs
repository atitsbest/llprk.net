using Llprk.Web.UI.Areas.Store;
using Llprk.Web.UI.Migrations;
using log4net.Config;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SimpleInjector;
using SimpleInjector.Integration.Web.Mvc;
using SimpleInjector.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Llprk.Web.UI
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var container = new Container();
            _InitDependencyInjection(container);

            // BasicConfigurator replaced with XmlConfigurator.
            log4net.Config.XmlConfigurator.Configure();

            AutoMapper.Mapper.AddProfile<AutoMapperProfile>();
            AutoMapper.Mapper.AddProfile<Llprk.Application.AutoMapperProfile>();

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            MappingConfig.RegisterMappings();

            // Create Json.Net formatter serializing DateTime using the ISO 8601 format
            var config = GlobalConfiguration.Configuration;
            var settings = config.Formatters.JsonFormatter.SerializerSettings;
            settings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Local;


			// Migrationen laufen lassen.
            //new _01OrderPriceInDBMigration().Execute();
        }

        /// <summary>
        /// Dependency Injection init.
        /// Alle Componenten registrieren, Integration mit ASP.NET MVC, ASP.NET WebApi.
        /// </summary>
        /// <param name="container"></param>
        private void _InitDependencyInjection(Container container)
        {
            if (container == null) throw new ArgumentNullException("container");

            container.RegisterPackages(new Type[] { 
                typeof(DependencyInjectionConfig),
                typeof(Llprk.Application.DependencyInjectionConfig),
            }.Select(t => Assembly.GetAssembly(t)));

            // Controller für DI registrieren.
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
            container.RegisterMvcIntegratedFilterProvider();

            container.Verify();

            // Container mit MVC verbinden.
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
        }
    }
}