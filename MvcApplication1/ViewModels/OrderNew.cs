using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Llprk.Web.UI.Models;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Llprk.Web.UI.ViewModels
{
    /// <summary>
    /// ViewModel für ~/orders/new
    /// </summary>
    // TODO: Validierung von "Order" übernehmen.
    public class OrderNew : PostOrderNew
    {
        public IEnumerable<SelectListItem> Countries { get; set; }

		/// <summary>
		/// CTR
		/// </summary>
		/// <param name="countries"></param>
        public OrderNew(IEnumerable<Country> countries) : base()
        {
            Countries = countries.Select(c => new SelectListItem() {
				Text = c.Name,
				Value = c.Id
            });
        }
    }
}