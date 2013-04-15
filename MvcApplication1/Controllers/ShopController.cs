using Llprk.Web.UI.Models;
using Llprk.Web.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Llprk.Web.UI.Application;
using Llprk.Web.UI.Application.Exceptions;
using PayPal;
using System.Configuration;
using PayPal.Api.Payments;
using System.Globalization;

namespace Llprk.Web.UI.Controllers
{
    public class ShopController : ApplicationController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int? id)
        {
            var viewModel = new ShopIndex();
            viewModel.Categories = db.Categories.ToList();
            viewModel.Products = db.Products
                       .Where(p => p.IsPublished
                                && (!id.HasValue || (p.CategoryId == id.Value))
                                && p.Available > 0) // Nur verfügbare Produkte anzeigen.
                       .ToArray();
            return View(viewModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Categories(int id)
        {
            var viewModel = new ShopCategory();
            viewModel.Category = db.Categories.First(c => c.Id == id);
            viewModel.Products = db.Products
                       .Where(p => p.IsPublished
                                && p.CategoryId == id
                                && p.Available > 0) // Nur verfügbare Produkte anzeigen.
                       .ToArray();
            return View(viewModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int id)
        {
            var viewModel = new ShopDetail();
            viewModel.Product = db.Products.Where(p => p.Id == id).FirstOrDefault();
            return View(viewModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public ActionResult New(OrderNew viewModel)
        {
            System.Threading.Thread.Sleep(500);
            Order order = null;

            if (ModelState.IsValid) {
                order = new Order() {
                    Address1 = viewModel.Address1,
                    Address2 = viewModel.Address2,
                    City = viewModel.City,
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

                try {
                    new ShopService().PlaceOrder(db, order, productIdsAndQtys);
                }
                catch (AppException e) {
                    Response.StatusCode = 500;
                    return Json(e.Message);
                }
            }
            else {
                Response.StatusCode = 500;
                return Json(ModelState.First().Value);
            }

            return Json(order.Id);
        }

        //[HttpPost]
        public ActionResult PayPal(int orderId) 
        {
            var order = db.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null) { 
				throw new ArgumentException(string.Format("Es gibt keinen Auftrag mit der Nummer '{0}'!", orderId));
            }
            if (order.IsPaid) {
                throw new Exception(string.Format("Die Bestellung {0} ist bereits bezahlt!", order.OrderNumber));
            }

			// Urls für PayPal generieren.
			var return_url = this.Url.Action("executepaypal", "shop", new { orderId = order.Id }, this.Request.Url.Scheme);
            var cancel_url = this.Url.Action("cancelpaypal", "shop", new { orderId = order.Id }, this.Request.Url.Scheme);

			var tokenCredential = _GetPayPalToken();

			string accessToken = tokenCredential.GetAccessToken();

			var amountDetails = new AmountDetails();
			amountDetails.subtotal = order.SubTotalPrice.ToString("n",new CultureInfo("en-US", false));
			amountDetails.tax = "0.00"; // Derzeit geben wir keine Steuern an.
			amountDetails.shipping = order.ShippingCosts.ToString("n", new CultureInfo("en-US", false));

			var amount = new Amount();
			amount.total = order.TotalPrice.ToString("n", new CultureInfo("en-US", false));
			amount.currency = "USD"; // TODO: EUR
			amount.details = amountDetails;

			var transaction = new Transaction();
			transaction.amount = amount;
			transaction.description = string.Format("Deine Bestellung {0} von lillypark.com", order.OrderNumber);

			var transactions = new List<Transaction>();
			transactions.Add(transaction);

			var payer = new Payer();
			payer.payment_method = "paypal";

			var payment = new Payment();
			payment.intent = "sale";
			payment.payer  = payer;
            payment.transactions = transactions;
            payment.redirect_urls = new RedirectUrls {
				return_url = return_url,
				cancel_url = cancel_url
            };

			Payment createdPayment = payment.Create(accessToken);

			if (createdPayment == null) {
				throw new Exception("Beim Bezahlen mit PayPal ist etwas schief gelaufen. Aber keine Angst, Dein Geld wurde nicht angefasst!");
            }

			// Wir können der Kunden an PayPal weiterleiten.
            if (createdPayment.state == "created") {
                var approvalUrl = (from link in createdPayment.links
                                   where link.rel == "approval_url"
                                   select link.href).FirstOrDefault();
                if (approvalUrl == null) {
                    throw new Exception("PayPal hat uns keinen Link gegeben mit dem du Deine Bezahlung bestätigen könntest. Probiere es später noch einmal.");
                }

				// PaymentId speichern.
                order.PayPalPaymentId = createdPayment.id;
                db.SaveChanges();

                return Redirect(approvalUrl);
            }

            throw new Exception("PayPal lässt Dich leider nicht zahlen. Hmmm....");
        }

		/// <summary>
		/// Der Kunde hat auf PayPal die Zahlung bestätigt und wird jetzt auf diese Url weiter geleitet.
		/// </summary>
		/// <param name="orderId"></param>
		/// <param name="token"></param>
		/// <param name="payerId"></param>
		/// <returns></returns>
        public ActionResult ExecutePayPal(int orderId, string token, string payerId)
        { 
            var order = db.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null) { 
				throw new ArgumentException(string.Format("Es gibt keinen Auftrag mit der Nummer '{0}'!", orderId));
            }

            var tokenCredential = _GetPayPalToken();
			string accessToken = tokenCredential.GetAccessToken();

			var payment = Payment.Get(accessToken, order.PayPalPaymentId);

			var paymentExecution = new PaymentExecution();
			paymentExecution.payer_id = payerId;

            var newPayment = payment.Execute(accessToken, paymentExecution);

            if (newPayment.state == "approved") {
                new ShopService().PayOrder(db, order, "TODO");
            }	

            return View(order);
        }

		/// <summary>
		/// Der Kunde hat die Bezahlung bei PayPal NICHT bestätigt.
		/// </summary>
		/// <param name="orderId"></param>
		/// <param name="token"></param>
		/// <param name="payerId"></param>
		/// <returns></returns>
        public ActionResult CancelPayPal(int orderId)
        {
            var order = db.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null) { 
				throw new ArgumentException(string.Format("Es gibt keinen Auftrag mit der Nummer '{0}'!", orderId));
            }

			// PaymentId wieder löschen.
            order.PayPalPaymentId = null;
            db.SaveChanges();

            ViewBag.OrderId = orderId;
            return View(order);
        }


		/// <summary>
		/// Helper: Holt von PayPal ein neues Accesstoken.
		/// </summary>
		/// <returns></returns>
		private static OAuthTokenCredential _GetPayPalToken()
		{
			var tokenCredential = new OAuthTokenCredential(
				ConfigurationManager.AppSettings["PayPal.ClientID"], // Client ID
				ConfigurationManager.AppSettings["PayPal.Secret"]); // Secret
			return tokenCredential;
		}

    }
}
