using Llprk.Web.UI.Application;
using PayPal;
using PayPal.Api.Payments;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
/*
namespace Llprk.Web.UI.Controllers
{
	/// <summary>
	/// Kümmert sich um die Bezahlung per PayPal mit alle dafür nötigen Schritten:
    ///		Create -> Confirmed/Canceled -> Execute -> Paid
	/// </summary>
    public class PayPalController : ApplicationController
    {
#if !DEBUG
        [HttpPost]
#endif
        public ActionResult Create(int orderId) 
        {
            var order = _GetUnpaidOrder(orderId);

			// Urls für PayPal generieren.
			var return_url = this.Url.Action("confirmed", "paypal", new { orderId = order.Id }, this.Request.Url.Scheme);
            var cancel_url = this.Url.Action("cancel", "paypal", new { orderId = order.Id }, this.Request.Url.Scheme);

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

			var createdPayment = payment.Create(accessToken);

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
		/// 
		/// </summary>
		/// <param name="orderId"></param>
		/// <returns></returns>
        public ActionResult Confirmed(int orderId, string token, string payerId)
        {
            var order = _GetUnpaidOrder(orderId);

            ViewBag.OrderId = orderId;
            ViewBag.Token = token;
            ViewBag.PayerId = payerId;
            return View(order);
        }


		/// <summary>
		/// Der Kunde hat auf PayPal die Zahlung bestätigt und wird jetzt auf diese Url weiter geleitet.
		/// </summary>
		/// <param name="orderId"></param>
		/// <param name="token"></param>
		/// <param name="payerId"></param>
		/// <returns></returns>
		[HttpPost]
        public ActionResult Execute(int orderId, string token, string payerId)
        {
            var order = _GetUnpaidOrder(orderId);
            if (string.IsNullOrWhiteSpace(token) ||
                string.IsNullOrWhiteSpace(payerId)) { throw new ArgumentException("Ungültige Anfrage!"); }

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
		/// 
		/// </summary>
		/// <param name="orderId"></param>
		/// <returns></returns>
        public ActionResult Paid(int orderId)
        {
            var order = db.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null) {
                throw new ArgumentException(string.Format("Es gibt keinen Auftrag mit der Nummer '{0}'!", orderId));
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
        public ActionResult Canceled(int orderId)
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

		/// <summary>
		/// Liefert den Auftrag zur ID, aber nur, wenn der Auftrag noch nicht bezahlt wurde.
		/// </summary>
		/// <param name="orderId"></param>
		/// <returns></returns>
        private Models.Order _GetUnpaidOrder(int orderId)
        {
            var order = db.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null) {
                throw new ArgumentException(string.Format("Es gibt keinen Auftrag mit der Nummer '{0}'!", orderId));
            }
            if (order.IsPaid) {
                throw new Exception(string.Format("Die Bestellung {0} ist bereits bezahlt!", order.OrderNumber));
            }
            return order;
        }

    }
}
*/
