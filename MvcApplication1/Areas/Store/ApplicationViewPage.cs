﻿using Llprk.DataAccess.Models;
using System.Linq;
using System.Web.Mvc;

namespace Llprk.Web.UI.Areas.Store
{
    public abstract class ApplicationViewPage<T> : WebViewPage<T>
    {
        protected override void InitializePage()
        {
            SetViewBagDefaultProperties();
            base.InitializePage();
        }

        private void SetViewBagDefaultProperties()
        {
            using (var db = new Entities()) {
                ViewBag.Categories = db.Categories.ToArray();
            } 
        }
    }
}