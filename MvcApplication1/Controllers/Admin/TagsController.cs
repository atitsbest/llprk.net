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
    public class TagsController : SingleValueController<Tag, int>
    {
        public TagsController()
            : base((e) => e.Tags)
        { }
    }
}