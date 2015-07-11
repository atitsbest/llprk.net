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
using System.IO;
using Llprk.Application.DTOs.Requests;

namespace Llprk.Web.UI.Areas.Admin.Controllers
{
    [Authorize]
    public partial class ThemesController : ApplicationController
    {
        /// <summary>
        /// CTR
        /// </summary>
        public ThemesController()
        {
        }

        //
        // GET: /Themes/
        public virtual ActionResult Index()
        {
            var viewModel = new ThemesRequest
            {
                Themes = Directory.EnumerateDirectories(Server.MapPath("~/Themes/")).Select(di =>
                    new ThemesRequest.Theme
                    {
                        Name = Path.GetFileName(di),
                        IsActive = false
                    })
            };

            return View(viewModel);
        }

        //
        // GET: /Themes/Edit/{id}
        public virtual ActionResult Edit(string id)
        {
            return View();
        }
    }
}