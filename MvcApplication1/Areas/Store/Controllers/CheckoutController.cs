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
using DotLiquid;
using Llprk.Web.UI.Areas.Store.Controllers.Helpers;
using Llprk.DataAccess.Models.Theme;
using Llprk.Web.UI.Filters;
using Llprk.Web.UI.Liquid;
using Llprk.Web.UI.Areas.Store.Models;

namespace Llprk.Web.UI.Areas.Store.Controllers
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public partial class CheckoutController : ApplicationController
    {
        private ICartService _CartService;
        private ITaxService _TaxService;
        private IShippingService _ShippingService;
        private ThemeService _ThemeService;

        /// <summary>
        /// CTR
        /// </summary>
        /// <param name="themes"></param>
        public CheckoutController(ICartService carts, ThemeService themes, ITaxService taxes, IShippingService shipping)
        {
            _CartService = carts;
            _ThemeService = themes;
            _TaxService = taxes;
            _ShippingService = shipping;
        }
        //
        // GET: /Checkout/

        [HttpGet, ThemeFilter]
        public virtual ActionResult Index()
        {
            if (Session["cartId"] == null) { 
                return RedirectToAction(MVC.Store.Cart.Index());
            }
            var cart = _CartService.GetCart((int)Session["cartId"]);

            // Cart empty?
            if (cart.LineItems.Count() == 0) { 
                return RedirectToAction(MVC.Store.Cart.Index());
            }


            var vm = Mapper.Map<CheckoutIndex>(cart);
            vm.Countries = Mapper.Map<CheckoutIndex.Country[]>(db.Countries);

            var checkoutHtml = RenderViewHelper.RenderViewToString(ControllerContext, "Index", vm);

            IThemeItem layoutItem;
            var theme = _ThemeService.GetTheme(ViewBag.ThemeName);
            if (ViewBag.Unpublished != null)
            {
                layoutItem = theme.GetUnpublishedItem("layout.checkout.liquid", "layouts")
                    ?? theme.GetUnpublishedItem("layout.liquid", "layouts");
            }
            else
            {
                layoutItem = theme.GetItem("layout.checkout.liquid", "layouts")
                    ?? theme.GetItem("layout.liquid", "layouts");
            }

            Template.FileSystem = new LiquidFileSystem(theme, ViewBag.Unpublished ?? false);

            Template.RegisterFilter(typeof(ScriptTagFilter));
            Template.RegisterFilter(typeof(StylesheetTagFilter));
            Template.RegisterFilter(typeof(ImageUrlFilter));

            // Template lesen. TODO: Cache.
            var layout = Template.Parse(layoutItem.ReadContent());

            return RenderTemplate(layout, checkoutHtml);
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

        [HttpGet]
        public virtual JsonResult VariableCosts(string country) 
        {
            if (string.IsNullOrWhiteSpace(country)) throw new ArgumentNullException("country");

            using (var db = new Entities())
            {
                var cart = _CartService.GetCart((int)Session["cartId"]);

                var c = db.Countries.Single(x => x.Id.ToLower() == country.ToLower());

                var shippingCosts = _ShippingService.CalculateShippingCosts(cart.Id, country);
                var tax = _TaxService.TaxForCountry(cart.Id, country);


                // INFO: Bewusst werden hier keine Nummern sondern formatierte Strings
                //       geliefert.
                //       Es sollen nicht Server und Client dem Benutzer Preise berechnen,
                //       denn die können auseinander liegen.
                return JsonNet(new
                {
                    ShippingCosts = shippingCosts.ToString("C"),
                    Tax = tax.ToString("C"),
                    Total = (cart.Subtotal + tax + shippingCosts).ToString("C")
                });
            }
        }
    }
}
