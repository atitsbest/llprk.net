using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Llprk.Web.UI.Models;

namespace Llprk.Web.UI.Controllers.Admin
{
    [Authorize]
    public class ParametersController : ApplicationController
    {
        public ActionResult Index() 
        {
            return View(db.Parameters.First());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Index(Parameter config)
        {
            var c= db.Parameters.First();
            c.MailMessagePaid = config.MailMessagePaid;
            c.MailMessageShipped = config.MailMessageShipped;
            db.SaveChanges();

            return RedirectToAction("index");
        }
    }
}