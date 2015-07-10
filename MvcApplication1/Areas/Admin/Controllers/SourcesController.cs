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
using Llprk.Application.Parameters;
using Llprk.Application;
using Llprk.Web.UI.Controllers.Results;

namespace Llprk.Web.UI.Areas.Admin.Controllers
{
    [Authorize]
    public partial class SourcesController : ApplicationController
    {
        //
        // GET: /Pages/
        [HttpGet]
        public virtual JsonResult Pages(QueryParams ps)
        {
            return JsonNet(new Result<Page>(ps, db.Pages));
        }
    }
}