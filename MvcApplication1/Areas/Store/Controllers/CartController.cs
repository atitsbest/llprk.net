using DotLiquid;
using Llprk.Web.UI.Areas.Store.Models;
using Llprk.Web.UI.Controllers;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Llprk.Web.UI.ViewModels;
using System.Collections.Generic;
using Llprk.Web.UI.Liquid;
using Llprk.Application.Services;
using System;
using Llprk.DataAccess.Models.Theme;
using Llprk.Web.UI.Filters;
using Llprk.DataAccess.Models;

namespace Llprk.Web.UI.Areas.Store.Controllers
{
    [ThemeFilter]
    public partial class CartController : ApplicationController
    {
        private ThemeService _ThemeService;
        private ICartService _CartService;

        /// <summary>
        /// CTR
        /// </summary>
        /// <param name="themes"></param>
        public CartController(ThemeService themes, ICartService carts)
        {
            if (themes == null) throw new ArgumentNullException("themes");
            if (carts == null) throw new ArgumentNullException("carts");

            _ThemeService = themes;
            _CartService = carts;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Index()
        {
            // TODO: Mach einen Filter oder sowas draus.
            int cartId;
            if (Session["cartId"] == null)
            {
                cartId = _CartService.CreateCart().Id;
                Session["cartId"] = cartId;
            }
            else
            {
                cartId = (int)Session["cartId"];
            }

            var viewModel = new CartIndex
            {
                LineItems = Mapper.Map<IEnumerable<LiquidLineItem>>(db.LineItems
                        .Include(li => li.Product)
                        .Where(li => li.CartId == cartId))
            };

            var theme = _ThemeService.GetTheme(ViewBag.ThemeName);
            IThemeItem layoutItem;
            IThemeItem templateItem;
            if (ViewBag.Unpublished != null)
            {
                layoutItem = theme.GetUnpublishedItem("layout.liquid", "layouts");
                templateItem = theme.GetUnpublishedItem("cart.liquid", "templates");
            }
            else
            {
                layoutItem = theme.GetItem("layout.liquid", "layouts");
                templateItem = theme.GetItem("cart.liquid", "templates");
            }

            Template.FileSystem = new LiquidFileSystem(theme, ViewBag.Unpublished);

            Template.RegisterFilter(typeof(ScriptTagFilter));
            Template.RegisterFilter(typeof(StylesheetTagFilter));
            Template.RegisterFilter(typeof(ImageUrlFilter));

            // Template lesen. TODO: Cache.
            var layout = Template.Parse(layoutItem.ReadContent());
            var template = Template.Parse(templateItem.ReadContent());

            // Render Template.
            template.Registers.Add("file_system", Template.FileSystem);
            var templateHtml = template.Render(Hash.FromAnonymousObject(new
            {
                cart = viewModel,
                cart_url = Url.Action(MVC.Store.Cart.Update()),
                page_title = "Warenkorb",
                template = "cart"
            }));

            return RenderTemplate(layout, templateHtml);
        }

        /// <summary>
        /// Add a product to the cart.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult Add(int id, int qty)
        {
            int cartId;
            if (Session["cartId"] == null)
            {
                cartId = _CartService.CreateCart().Id;
                Session["cartId"] = cartId;
            }
            else
            {
                cartId = (int)Session["cartId"];
            }

            _CartService.AddProduct(cartId, id, qty);

            return RedirectToAction(MVC.Store.Cart.Index());
        }

        /// <summary>
        /// Update cart information (quantities, ...)
        /// </summary>
        /// <returns></returns>
        [HttpPost, HttpParamAction]
        public virtual ActionResult Update()
        {
            return RedirectToAction(MVC.Store.Cart.Index());
        }

        /// <summary>
        /// Proceede to checkout.
        /// </summary>
        /// <returns></returns>
        [HttpPost, HttpParamAction]
        public virtual ActionResult Checkout()
        {
            return RedirectToAction(MVC.Store.Checkout.Index());
        }
    }

}
