using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Llprk.Web.UI.Models;
using AutoPoco;
using AutoMapper;
using Llprk.Web.UI.ViewModels;
using System.Web.UI;

namespace Llprk.Web.UI.Controllers.Admin
{
    [Authorize, OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
    public class ParametersController : ApplicationController
    {
        public ActionResult Index()
        {
            return View(
                Mapper.Map<ParameterIndex>(db.Parameters.First())
            );
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Index(ParameterIndex config)
        {
            if (ModelState.IsValid) {
                var c = db.Parameters.First();
                c.MailMessageOrdered = config.MailMessageOrdered;
                c.MailMessagePaid = config.MailMessagePaid;
                c.MailMessageShipped = config.MailMessageShipped;
                db.SaveChanges();

		return RedirectToAction("index");
            }

	    // Neu anzeigen mit Fehlern.
            return View(config);
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult IsMailTemplateValid()
        {
            var pocoFactory = AutoPocoContainer.Configure(x => {
                x.Conventions(c => c.UseDefaultConventions());
                x.AddFromAssemblyContainingType<Order>();
                x.Include<Order>();
            });
            var pocoSession = pocoFactory.CreateSession();

            var order = pocoSession.Single<Order>();
            var template = Request.Unvalidated.Form[0];

            try {
                Nustache.Core.Render.StringToString(template, order);
            }
            catch (Exception e) {
                return Json(string.Format("Invalid email template ({0})!", e.Message), JsonRequestBehavior.AllowGet);
            }

	    // Alles ok.
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}