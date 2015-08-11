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
            using (var db = new Entities())
            {
                var sut = _createSut();

                sut.UpdateCountryTax("de", 22);

                Assert.AreEqual(22, db.Taxes.Single(t => t.CountryId == "de").Percent);
            }
        }

        [TestMethod]
        public void Calculate_Tax_for_Country()
        {
            using (var db = new Entities())
            {
                var sut = _createSut();

                using (new TransactionScope())
                {
                    // Set Tax.
                    setTaxForCountry(db, "at", 20);

                    var cartService = new CartService();
                    var cart = cartService.CreateCart();
                    var product = db.Products.First(p => p.ChargeTaxes);
                    var product2 = db.Products.First(p => !p.ChargeTaxes);
                    product.Price = 10;
                    product2.Price = 10;
                    db.SaveChanges();

                    cartService.AddProduct(cart.Id, product.Id, 2);
                    cartService.AddProduct(cart.Id, product2.Id, 2);

                    var result = sut.TaxForCountry(cart.Id, "at");

                    Assert.AreEqual(3.3f, (float)result, 0.04f);
                }
            }
        }

        private void setTaxForCountry(Entities db, string v1, int v2)
        {
            var tax = db.Taxes.SingleOrDefault(t => t.CountryId == "at");
            tax.Percent = 20;
            if (tax == null)
            {
                tax.CountryId = "at";
                db.Taxes.Add(tax);
            }
            db.SaveChanges();
        }

        private TaxService _createSut()
        {
            return new TaxService();
        }
    }
}
