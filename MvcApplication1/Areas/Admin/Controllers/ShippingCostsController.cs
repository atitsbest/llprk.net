﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Llprk.DataAccess.Models;
using System.Text.RegularExpressions;

namespace Llprk.Web.UI.Areas.Admin.Controllers
{
    [Authorize]
    public partial class ShippingCostsController : SingleValueController<ShippingCategory, int>
    {
        public ShippingCostsController()
            : base((e) => e.ShippingCategories)
        { }

		[HttpPost]
        public virtual ActionResult ShippingCosts()
        {
            if (ModelState.IsValid) { 
				var scKeys = Request.Form.AllKeys.Where(k => k.StartsWith("sc_"));

				foreach (var key in scKeys) { 
					var match = Regex.Match(key, @"^sc_([a-z]*)_(\d*)$", RegexOptions.IgnoreCase);
					var countryId = match.Groups[1].Value;
					var categoryId = int.Parse(match.Groups[2].Value);
					var amount = decimal.Parse(Request.Form[key]);

					// Bestehende Kosten finden.
					var cost = (from sc in db.ShippingCosts
								where sc.ShippingCategoryId == categoryId && sc.CountryId == countryId
									select sc).FirstOrDefault();

					if (cost == null) {
						// Neu anlegen.
						db.ShippingCosts.Add(new ShippingCost() {
							CountryId = countryId,
							ShippingCategoryId = categoryId,
							Amount = amount
						});
					}
					else {
						// Update
						cost.Amount = amount;
					}
				}

				db.SaveChanges();
            }
            return RedirectToAction("index"); 
        }
    }
}