using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Llprk.DataAccess.Models;

namespace Llprk.Web.UI.Areas.Admin.Controllers
{
    [Authorize]
    public partial class TagsController : SingleValueController<Tag, int>
    {
        public TagsController()
            : base((e) => e.Tags)
        { }
    }
}