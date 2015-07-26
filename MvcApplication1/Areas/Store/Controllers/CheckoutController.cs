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

namespace Llprk.Web.UI.Areas.Store.Controllers
{
    public partial class CheckoutController : ApplicationController
    {
        private ICartService _CartService;
        private ThemeService _ThemeService;

        /// <summary>
        /// CTR
        /// </summary>
        /// <param name="themes"></param>
        public CheckoutController(ICartService carts, ThemeService themes)
        {
            if (carts == null) throw new ArgumentNullException("carts");
            if (themes == null) throw new ArgumentNullException("themes");

            _CartService = carts;
            _ThemeService = themes;
        }
        //
        // GET: /Checkout/

        [HttpGet, ThemeFilter]
        public virtual ActionResult Index()
        {
            var vm = new OrderNew(db.Countries.ToList());
            var cart = _CartService.GetCart((int)Session["cartId"]);
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

            Template.FileSystem = new LiquidFileSystem(theme, ViewBag.Unpublished);

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
    }
}
