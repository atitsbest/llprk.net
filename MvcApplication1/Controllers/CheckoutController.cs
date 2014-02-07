using AutoMapper;
using Llprk.Web.UI.Application;
using Llprk.Web.UI.Application.Exceptions;
using Llprk.Web.UI.Models;
using Llprk.Web.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Llprk.Web.UI.Controllers
{
    public class CheckoutController : ApplicationController
    {
        //
        // GET: /Checkout/

        [HttpGet]
        public ActionResult Index()
        {
            var vm = new OrderNew(db.Countries.ToList());
            return View(vm);
        }


        [HttpPost]
        public ActionResult Index(PostOrderNew viewModel)
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
        public ActionResult Success(Guid id) 
        {
            return View();
        }

        [HttpGet]
        public ActionResult Thankyou() 
        {
            return View();
        }
    }
}
