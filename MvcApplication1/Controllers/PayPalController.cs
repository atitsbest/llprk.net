using Llprk.Web.UI.Application;
using Llprk.Web.UI.Payments;
using PayPal;
using PayPal.Api.Payments;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Llprk.Web.UI.Controllers
{
	/// <summary>
	/// Kümmert sich um die Bezahlung per PayPal mit alle dafür nötigen Schritten:
    ///		Create -> Confirmed/Canceled -> Execute -> Paid
	/// </summary>
    public class PayPalController : ApplicationController
    {
        private PayPalPayment _PayPal;

		/// <summary>
		/// CTR
		/// </summary>
        public PayPalController()
        {
            _PayPal = new PayPalPayment();
        }

#if !DEBUG
        [HttpPost]
#endif
        public ActionResult Create(int orderId) 
        {
            var order = _GetUnpaidOrder(orderId);

            // Urls für PayPal generieren.
            var return_url = new Uri(this.Url.Action("confirmed", "paypal", new { orderId = order.Id }, this.Request.Url.Scheme));
            var cancel_url = new Uri(this.Url.Action("canceled", "paypal", new { orderId = order.Id }, this.Request.Url.Scheme));

            var description = string.Format("Deine Bestellung {0} von lillypark.com", order.OrderNumber);
            string paymentId;

            var approvalUrl = _PayPal.CreateNewPayment(
                return_url,
                cancel_url,
                subTotalPrice: order.SubTotalPrice,
                shippingCosts: order.ShippingCosts,
                totalPrice: order.TotalPrice,
                description: description,
                paymentId: out paymentId);

            // PaymentId speichern.
            order.PayPalPaymentId = paymentId;
            db.SaveChanges();

            return Redirect(approvalUrl.AbsoluteUri);
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

            var context = _PayPal.GetPayPalToken();
            //string accessToken = tokenCredential.GetAccessToken();

			var payment = Payment.Get(context, order.PayPalPaymentId);

			var paymentExecution = new PaymentExecution();
			paymentExecution.payer_id = payerId;

            var newPayment = payment.Execute(context, paymentExecution);

            if (newPayment.state == "approved") {
                new ShopService().PayOrder(db, order);
            }
            // Kann sein, dass die Zahlung gemacht, aber von PayPal
            // noch nicht bestätigt ist => Danke für die Bestellung,
            // aber Lilly muss noch die Zahlung überprüfen.
            else {
                order.Comment += "\nACHTUNG: Zahlung von PayPal nicht 'approved'. Bitte im PayPal-Account kontrollieren.";
                db.SaveChanges();
            }

            return RedirectToAction("thankyou", "checkout");
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
            order.Comment += "ACHTUNG: Bezahlung per PayPal wurde abgebrochen. Die Bestellung ist noch nicht bezahlt. Wenn sich der Kunde bis " + DateTime.Now.AddDays(2).ToShortDateString() + " nicht meldet, kann die Bestellung gelöscht werden.";
            db.SaveChanges();

            ViewBag.OrderId = orderId;
            return View(order);
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

