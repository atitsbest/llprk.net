using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Llprk.Web.UI.Payments;
using PayPal.Api.Payments;

namespace llprk.Tests.Payments
{
    [TestClass]
    public class PayPalTest
    {
        [TestMethod]
        public void Should_create_new_payment()
        {
            var sut = new PayPalPayment();
            string paymentId;

            var paypalUrl = sut.CreateNewPayment(
                returnUrl: new Uri("http://localhost:8080"), 
                cancelUrl: new Uri("http://localhost:8080"), 
                subTotalPrice: 9.99M, 
                shippingCosts: 3.50M, 
                totalPrice: 13.49M, 
                description: "+++ Das ist ein Test von llprk. +++",
                paymentId: out paymentId);

            Assert.IsNotNull(paypalUrl);
            Assert.IsTrue(paypalUrl.AbsoluteUri.IndexOf("sandbox") != -1);
            Assert.IsNotNull(paymentId);
        }
    }
}
