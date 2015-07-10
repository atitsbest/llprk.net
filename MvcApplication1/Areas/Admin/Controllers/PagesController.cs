using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Llprk.DataAccess.Models;
using AutoMapper;
using Llprk.Web.UI.Application;
using Llprk.Web.UI.Controllers;
using Llprk.Web.UI.Areas.Admin.Models;
using Llprk.Application.DTOs.Responses;
using Llprk.Application.Services;
using Llprk.Web.UI.Controllers.Results;

namespace Llprk.Web.UI.Areas.Admin.Controllers
{
    [Authorize]
    public partial class PagesController : ApplicationController
    {
        protected IPageService _PageService;

        /// <summary>
        /// CTR
        /// </summary>
        public PagesController()
        {
            _PageService = new PageService();
        }

        //
        // GET: /Pages/
        public virtual ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Pages/
        public virtual ActionResult New()
        {
            return View();
        }

        //
        // GET: /Pages/Edit/5
        public virtual ActionResult Edit(int id)
        {
            var vm = _PageService.GetPageForEdit(id);
            return View(vm);
        }

        [HttpPost]
        public virtual ActionResult Create(NewPageResponse info)
        {
            if (ModelState.IsValid)
            {
                _PageService.CreatePage(info);
                return new EntityResult(MVC.Admin.Pages.Index(), string.Format("Page '{0}' added.", info.Title));
            }
            return new Http400Result(ModelState);
        }

        [HttpPost]
        public virtual ActionResult Update(EditPageResponse info)
        {
            if (ModelState.IsValid)
            {
                _PageService.UpdatePage(info);
                return new EntityResult(MVC.Admin.Pages.Index(), string.Format("Page '{0}' saved.", info.Title));
            }
            return new Http400Result(ModelState);
        }

        //
        // GET: /Pages/Delete/5
        public virtual ActionResult Delete(int id = 0)
        {
            return View();
        }

        //
        // POST: /Pages/Delete/5

        [HttpPost, ActionName("Delete")]
        public virtual ActionResult DeleteConfirmed(int id)
        {
            return View();
        }
    }
}