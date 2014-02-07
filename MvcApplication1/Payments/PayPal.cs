using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using System.Net;
using System.IO;
using System.Globalization;
using PayPal;
using System.Configuration;
using PayPal.Api.Payments;
using PayPal.OpenIdConnect;

namespace Llprk.Web.UI.Payments
{

    public class PayPalPayment
    {
		/// <summary>
		/// Erstellt ein neues Payment für Paypal.
		/// </summary>
		/// <param name="returnUrl"></param>
		/// <param name="cancelUrl"></param>
		/// <param name="subTotalPrice"></param>
		/// <param name="shippingCosts"></param>
		/// <param name="totalPrice"></param>
		/// <param name="description"></param>
		/// <returns>Die Url für Paypal auf die der Benutzer weiter geleitet werden muss.</returns>
        public Uri CreateNewPayment(Uri returnUrl, Uri cancelUrl, decimal subTotalPrice, decimal shippingCosts, decimal totalPrice, string description, out string paymentId) 
        {
			var context = GetPayPalToken();

            //string accessToken = tokenCredential.GetAccessToken();

			var amountDetails = new Details();
			amountDetails.subtotal = _toPayPalPrice(subTotalPrice);
			amountDetails.tax = "0.00"; // Derzeit geben wir keine Steuern an.
			amountDetails.shipping = _toPayPalPrice(shippingCosts);

			var amount = new Amount();
			amount.total = _toPayPalPrice(totalPrice);
            amount.currency = "EUR";
			amount.details = amountDetails;

			var transaction = new Transaction();
			transaction.amount = amount;
            transaction.description = description;

			var transactions = new List<Transaction>();
			transactions.Add(transaction);

			var payer = new Payer();
			payer.payment_method = "paypal";

			var payment = new Payment();
			payment.intent = "sale";
			payment.payer  = payer;
            payment.transactions = transactions;
            payment.redirect_urls = new RedirectUrls {
				return_url = returnUrl.AbsoluteUri,
				cancel_url = cancelUrl.AbsoluteUri
            };

			var createdPayment = payment.Create(context);

			if (createdPayment == null) {
				throw new Exception("Beim Bezahlen mit PayPal ist etwas schief gelaufen. Aber keine Angst, Dein Geld wurde nicht angefasst!");
            }

			// Wir können den Kunden an PayPal weiterleiten.
            if (createdPayment.state == "created") {
                var approvalUrl = (from link in createdPayment.links
                                   where link.rel == "approval_url"
                                   select link.href).FirstOrDefault();
                if (approvalUrl == null) {
                    throw new Exception("PayPal hat uns keinen Link gegeben mit dem du Deine Bezahlung bestätigen könntest. Probiere es später noch einmal.");
                }

#if DEBUG
				if (approvalUrl.ToLower().IndexOf("sandbox") == -1) throw new AccessViolationException("PAYPAYL ist NICHT im SANDBOX-Modus!");
#endif

                paymentId = createdPayment.id;
                return new Uri(approvalUrl);
            }

            throw new Exception("PayPal lässt Dich leider nicht zahlen. Hmmm....");
        }

		/// <summary>
		/// Helper: Holt von PayPal ein neues Accesstoken.
		/// </summary>
		/// <returns></returns>
		public APIContext GetPayPalToken()
		{
            var config = new Dictionary<string, string>();
            config.Add("clientId", ConfigurationManager.AppSettings["PayPal:Username"]);
            config.Add("clientSecret", ConfigurationManager.AppSettings["PayPal:Password"]);
            if (ConfigurationManager.AppSettings["PayPal:Sandbox"] != "False") {
                config.Add("mode", "sandbox");
            }

            var tokenCredential = new OAuthTokenCredential(
                config["clientId"],
                config["clientSecret"],
                config);

            var apiContext = new APIContext(tokenCredential.GetAccessToken());
            apiContext.Config = config;

			return apiContext;
		}

        private static string _toPayPalPrice(decimal val) 
        { 
            return val.ToString("n", new CultureInfo("en-US", false));
        }
    }
}