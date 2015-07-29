using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Llprk.DataAccess.Models;
using Llprk.Application.Services;
using System.Transactions;
using Llprk.Application.DTOs.Requests;

namespace llprk.Application.Test.Services
{
    [TestClass]
    public class ShippingServiceTest : TestBase
    {
        [TestMethod]
        public void Update_ShippingCategory()
        {
            using (new TransactionScope())
            {
                var db = new Entities();
                var sut = _createSut();

                var catId = db.ShippingCategories.First().Id;
                sut.UpdateShippingCategory(catId, "das ist ein test");

                Assert.AreEqual("das ist ein test", db.ShippingCategories.Single(c => c.Id == catId).Name);
            }
        }

        [TestMethod]
        public void Create_ShippingCategory()
        {
            var db = new Entities();
            var sut = _createSut();

            AssertChangedBy(db.ShippingCategories, () =>
            {
                sut.CreateShippingCategory("_delme");
            });
        }

        [TestMethod]
        public void Delete_ShippingCategory()
        {
            var db = new Entities();
            var sut = _createSut();

            AssertChangedBy(db.ShippingCategories, () =>
            {
                var cat = sut.CreateShippingCategory("tester");

                sut.DeleteShippingCost(cat.Id);
            }, 0);
        }

        [TestMethod]
        public void Update_Shipping_Costs_simple()
        {
            var db = new Entities();
            var sut = _createSut();
            var scId = db.ShippingCategories.First().Id;

            using (new TransactionScope())
            {
                sut.UpdateShippingCosts(new UpdateShippingCostsRequest { 
                    Countries = new UpdateShippingCostsRequest.Country[] {
                        new UpdateShippingCostsRequest.Country { 
                            Id = "at", 
                            ShippingCosts = new UpdateShippingCostsRequest.ShippingCost[] {
                                new UpdateShippingCostsRequest.ShippingCost {
                                    ShippingCategoryId = scId,
                                    Amount = 9, AdditionalAmount = 10
                                }
                            }
                        }
                    }
                });

                Assert.AreEqual(9, db.Countries.Single(c => c.Id == "at").ShippingCosts.Single(s => s.ShippingCategoryId == scId).Amount);
            }
        }

        [TestMethod]
        public void Calculate_Shipping_Costs()
        {
            using (new TransactionScope())
            {
                var db = new Entities();
                var sut = _createSut();
                var cartService = new CartService();

                var product = db.Products.First(p => p.Id == 1);
                cartService.AddProduct(1, product.Id, 17);

                // Act.
                var resultAt = sut.CalculateShippingCosts(1, "at");
                var resultDe = sut.CalculateShippingCosts(1, "de");
                
                // Assert
                Assert.AreEqual(6.75m + 16 * 1.5m, resultAt);
                Assert.AreEqual(8m + 16 * 2.0m, resultDe);
                
                // Add another product with a different shipping category.
                product = db.Products.First(p => p.Id == 5);
                cartService.AddProduct(1, product.Id, 2);

                // Act.
                resultAt = sut.CalculateShippingCosts(1, "at");
                resultDe = sut.CalculateShippingCosts(1, "de");
                
                // Assert
                Assert.AreEqual((6.75m + 16 * 1.5m) + (1.1m * 2), resultAt);
                Assert.AreEqual((8m + 16 * 2.0m) + (2.2m *2), resultDe);
            }
        }

        /// <summary>
        /// Create SUT
        /// </summary>
        /// <returns></returns>
        private ShippingService _createSut()
        {
            return new ShippingService();
        }
    }
}
