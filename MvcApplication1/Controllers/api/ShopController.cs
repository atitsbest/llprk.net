using Llprk.Web.UI.Application;
using Llprk.Web.UI.Application.Exceptions;
using Llprk.Web.UI.Models;
using Llprk.Web.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Llprk.Web.UI.Controllers.api
{
    public class ShopController : ApiController
    {
        protected Entities db = new Entities();

        [HttpPost]
        public int New(OrderNew viewModel)
        {
            Order order = null;

            order = new Order() {
                Address1 = viewModel.Address1,
                Address2 = viewModel.Address2,
                City = viewModel.City,
                CountryId = viewModel.CountryCode,
                Country = db.Countries.First(c => c.Id == viewModel.CountryCode.ToLower()),
                Email = viewModel.Email,
                Firstname = viewModel.Firstname,
                Name = viewModel.Name,
                Salutation = viewModel.Salutation,
                Zip = viewModel.Zip,
                Comment = viewModel.Comment
            };

            var productIdsAndQtys = viewModel.Products.ToDictionary(
                k => k.Id,
                v => v.Qty);

            new ShopService().PlaceOrder(db, order, productIdsAndQtys);

            return order.Id;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
