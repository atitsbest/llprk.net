using Llprk.Web.UI.Application;
using Llprk.Web.UI.Payments;
using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

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

			var redirect = PayPal.ExpressCheckout(new PayPalOrder { Amount = order.TotalPrice }, return_url, cancel_url);

            order.PayPalPaymentId = redirect.Token;
            db.SaveChanges();

			return new RedirectResult(redirect.Url);
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="orderId"></param>
		/// <returns></returns>
        public ActionResult Confirmed(int orderId)
        {
            var order = _GetUnpaidOrder(orderId);

            ViewBag.OrderId = orderId;
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

