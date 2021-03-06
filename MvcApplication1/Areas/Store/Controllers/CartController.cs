﻿using DotLiquid;
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
            int cartId = _EnsureCart();

            var viewModel = new CartIndex
            {
                LineItems = Mapper.Map<IEnumerable<LiquidLineItem>>(db.LineItems
                        .Include(li => li.Product)
                        .Where(li => li.CartId == cartId))
            };

            Template layout;
            Template template;
            var theme = _ThemeService.GetTheme(ViewBag.ThemeName);
            PrepareRenderTemplate(theme, "cart.liquid", out layout, out template);

            var templateHtml = template.Render(Hash.FromAnonymousObject(new
            {
                cart = viewModel,
                cart_url = Url.Action("action"), // Spielt mit den HttpParamAction von Update und Checkout zusammen.
                update_line_item_url = Url.Action(MVC.Store.Cart.Change()),
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
            var cartId = _EnsureCart();

            _CartService.AddProduct(cartId, id, qty);

            return RedirectToAction(MVC.Store.Cart.Index());
        }

        /// <summary>
        /// Update cart information (quantities, ...)
        /// </summary>
        /// <returns></returns>
        [HttpPost, HttpParamAction]
        public virtual ActionResult Update(CartUpdate info)
        {
            var cartId = _EnsureCart();

            _CartService.UpdateLineItemQty(cartId, info.Updates);
            return RedirectToAction(MVC.Store.Cart.Index());
        }

        /// <summary>
        /// Update a single line item, not the whole cart like Update().
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Change(int lineItemId, int qty)
        {
            var cartId = _EnsureCart();

            _CartService.UpdateLineItemQty(cartId, lineItemId, qty);
            return RedirectToAction(MVC.Store.Cart.Index());
        }

        /// <summary>
        /// Proceede to checkout.
        /// </summary>
        /// <returns></returns>
        [HttpPost, HttpParamAction]
        public virtual ActionResult Checkout(CartUpdate info)
        {
            var cartId = _EnsureCart();

            _CartService.UpdateLineItemQty(cartId, info.Updates);
            return RedirectToAction(MVC.Store.Checkout.Index());
        }

        /// <summary>
        /// We ne a cart. Existent (in Sesson) or a new one.
        /// </summary>
        /// <returns></returns>
        private int _EnsureCart()
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
            return cartId;
        }
    }

}
