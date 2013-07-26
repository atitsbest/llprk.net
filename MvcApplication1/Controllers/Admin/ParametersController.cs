using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Llprk.Web.UI.Models;
using AutoMapper;
using Llprk.Web.UI.ViewModels;
using System.Web.UI;
using FizzWare.NBuilder;

namespace Llprk.Web.UI.Controllers.Admin
{
    [Authorize, OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
    public class ParametersController : ApplicationController
    {
        public ActionResult Index()
        {
            ViewBag.DummyOrder = _CreateDummyOrder();

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
        public ActionResult Render(string template) { 
            var order = _CreateDummyOrder();
			string result = "";

            try {
                result = Nustache.Core.Render.StringToString(template, order);
            }
            catch (Exception e) {
                result = string.Format("Invalid email template ({0})!", e.Message);
            }

            // Alles ok.
            return Content(result);
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult IsMailTemplateValid()
        {
            var order = _CreateDummyOrder();
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

		/// <summary>
		/// Bsp. Bestellung erstellen.
		/// </summary>
		/// <returns></returns>
        private Order _CreateDummyOrder()
        {
            var order = Builder<Order>.CreateNew()
                .With(o => o.OrderLines = Builder<OrderLine>.CreateListOfSize(2)
					.All()
					.With(ol => ol.Product = Builder<Product>.CreateNew()
                        .With(p => p.Price = 44.99m)
                        .Build())
					.Build())
                .Build();
            return order;
        }
    }
}