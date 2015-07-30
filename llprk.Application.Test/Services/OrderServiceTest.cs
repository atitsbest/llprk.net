using Llprk.Application.DTOs.Requests;
using Llprk.Application.Services;
using Llprk.DataAccess.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace llprk.Application.Test.Services
{
    [TestClass]
    public class OrderServiceTest : TestBase
    {
        [TestMethod]
        public void Create_Order_simple()
        {
            using (var db = new Entities())
            {
                var sut = _createSut();
                var address = new CreateOrderRequest.Address {
                    Address1 = "Address1",
                    Address2 = "Address2",
                    City = "City",
                    CountryId = "at",
                    Email = "email@email.email",
                    Firstname = "Firstname",
                    Lastname = "Lastname",
                    Salutation = "Salut",
                    Zip = "1234"
                };
                var request = new CreateOrderRequest
                {
                    DeliveryAddress = address,
                };

                AssertChangedBy(db.Orders, () =>
                {
                    var order = sut.CreateOrder(3, request);
                    Assert.IsNotNull(order);
                });
            }
        }
        private OrderService _createSut()
        {
            return new OrderService(
                new ShippingService(),
                new TaxService());
        }
    }
}
