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
    public class CartServiceTest : TestBase
    {
        [TestMethod]
        public void Create_Cart()
        {
            var db = new Entities();
            var sut = _createSut();

            AssertChangedBy(db.Carts, () =>
            {
                var cart = sut.CreateCart();
                Assert.IsNotNull(cart);
            });
        }

        [TestMethod]
        public void Get_Cart()
        {
            var db = new Entities();
            var sut = _createSut();

            var cart = sut.GetCart(1);

            Assert.IsNotNull(cart);
            Assert.AreEqual(1, cart.Id);
        }

        [TestMethod]
        public void Add_Product_to_Cart()
        {
            var db = new Entities();
            var sut = _createSut();

            AssertChangedBy(db.LineItems, () =>
            {
                var product = db.Products.First();

                var lineItemId = sut.AddProduct(1, product.Id, 17);

                // Assert
                var cart = sut.GetCart(1);
                Assert.AreEqual(1, cart.LineItems.Count());
                Assert.AreEqual(product.Id, cart.LineItems.First().ProductId);
                Assert.AreEqual(17, cart.LineItems.First().Qty);
                Assert.AreNotEqual(0, lineItemId);
            });
        }

        [TestMethod]
        public void Add_the_same_Product_to_Cart()
        {
            var db = new Entities();
            var sut = _createSut();

            AssertChangedBy(db.LineItems, () =>
            {
                // Add product once...
                var product = db.Products.First();
                var lineItemId = sut.AddProduct(1, product.Id, 17);
                // Add same product a second time.
                var lineItem2Id = sut.AddProduct(1, product.Id, 1);

                // Assert
                var cart = sut.GetCart(1);
                Assert.AreEqual(1, cart.LineItems.Count());
                Assert.AreEqual(product.Id, cart.LineItems.First().ProductId);
                Assert.AreEqual(18, cart.LineItems.First().Qty);
                Assert.AreNotEqual(0, lineItemId);
                Assert.AreEqual(lineItem2Id, lineItemId);
            });
        }

        [TestMethod]
        public void Update_line_item_qty()
        {
            var db = new Entities();
            var sut = _createSut();

            using (new TransactionScope())
            {
                // Add product once...
                var product = db.Products.First();
                var lineItemId = sut.AddProduct(1, product.Id, 17);
                // Add same product a second time.
                sut.UpdateLineItemQty(1, lineItemId, 2);

                Assert.AreEqual(2, db.LineItems.Single(l => l.Id == lineItemId).Qty);
            }
        }

        [TestMethod]
        public void Update_line_item_qty_to_zero()
        {
            var db = new Entities();
            var sut = _createSut();

            using (new TransactionScope())
            {
                // Add product once...
                var product = db.Products.First();
                var lineItemId = sut.AddProduct(1, product.Id, 17);
                // Add same product a second time.
                sut.UpdateLineItemQty(1, lineItemId, 0);

                Assert.IsNull(db.LineItems.SingleOrDefault(l => l.Id == lineItemId));
            }
        }

        private CartService _createSut()
        {
            return new CartService();
        }
    }
}
