using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Llprk.DataAccess.Models;
using Llprk.Application.Services;
using System.Transactions;

namespace llprk.Application.Test.Services
{
    [TestClass]
    public class TaxServiceTest : TestBase
    {
        [TestMethod]
        public void Update_Country_Tax()
        {
            var db = new Entities();
            var sut = _createSut();

            sut.UpdateCountryTax("de", 22);

            Assert.AreEqual(22, db.Taxes.Single(t => t.CountryId == "de").Percent);
        }

        [TestMethod]
        public void Calculate_Tax_for_Country()
        {
            var db = new Entities();
            var sut = _createSut();

            using (new TransactionScope())
            {
                var cartService = new CartService();
                var cart = cartService.CreateCart();
                var product = db.Products.First(p => p.ChargeTaxes);
                var product2 = db.Products.First(p => !p.ChargeTaxes);
                product.Price = 10;
                product2.Price = 10;
                db.SaveChanges();
                cartService.AddProduct(cart.Id, product.Id, 2);
                cartService.AddProduct(cart.Id, product2.Id, 2);

                var tax = sut.TaxForCountry(cart.Id, "at");

                Assert.AreEqual(3.3f, (float)tax, 0.04f);
            }
        }

        private TaxService _createSut()
        {
            return new TaxService();
        }
    }
}
