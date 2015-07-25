using AutoMapper;
using Llprk.Web.UI.Application;
using Llprk.Web.UI.Application.Exceptions;
using Llprk.Web.UI.Areas.Admin.Models;
using Llprk.Web.UI.Controllers;
using Llprk.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Llprk.Application.Services;

namespace Llprk.Web.UI.Areas.Store.Controllers
{
    public partial class CheckoutController : ApplicationController
    {
        private ICartService _CartService;

        /// <summary>
        /// CTR
        /// </summary>
        /// <param name="themes"></param>
        public CheckoutController(ICartService carts)
        {
            if (carts == null) throw new ArgumentNullException("carts");

            _CartService = carts;
        }
        //
        // GET: /Checkout/

        [HttpGet]
        public virtual ActionResult Index()
        {
            var vm = new OrderNew(db.Countries.ToList());
            var cart = _CartService.GetCart((int)Session["cart_id"]);

            return View(vm);
        }


        [HttpPost]
        public virtual ActionResult Index(PostOrderNew viewModel)
        {
            if (ModelState.IsValid) {

                try {
                    var order = new Order() {
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

                    return viewModel.UsesPayPal
                        ? RedirectToAction("create", "paypal", new { orderId = order.Id })
                        : RedirectToAction("success", new { id = order.Id });

                }
                catch (AppException e) {
                    Error(e.Message);
                }
            }

			// ViewModel um die Länder anreichern.
            var vm = new OrderNew(db.Countries);
            return View("index", Mapper.Map(viewModel, vm));
        }


        [HttpGet]
        public virtual ActionResult Success(Guid id) 
        {
            return View();
        }

        [HttpGet]
        public virtual ActionResult Thankyou() 
        {
            return View();
        }
    }
}
