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
                int catId = 0;
                using (var db = new Entities())
                {
                    var sut = _createSut();

                    catId = db.ShippingCategories.First().Id;
                    sut.UpdateShippingCategory(catId, "das ist ein test");
                }

                // Komisch: Damit die geänderte Kateogrie auch wirklich geladen wird, muss der EF-Kontext
                //          neu erstellt werden. Identity-Map schlägt zu? Warum nur hier?
                using (var db = new Entities())
                {
                    Assert.AreEqual("das ist ein test", db.ShippingCategories.Single(c => c.Id == catId).Name);
                }
            }
        }

        [TestMethod]
        public void Create_ShippingCategory()
        {
            using (var db = new Entities())
            {
                var sut = _createSut();

                AssertChangedBy(db.ShippingCategories, () =>
                {
                    sut.CreateShippingCategory("_delme");
                });
            }
        }

        [TestMethod]
        public void Delete_ShippingCategory()
        {
            using (var db = new Entities())
            {
                var sut = _createSut();

                AssertChangedBy(db.ShippingCategories, () =>
                {
                    var cat = sut.CreateShippingCategory("tester");

                    sut.DeleteShippingCost(cat.Id);
                }, 0);
            }
        }

        [TestMethod]
        public void Update_Shipping_Costs_simple()
        {
            using (var db = new Entities())
            {
                var sut = _createSut();
                var scId = db.ShippingCategories.First().Id;

                using (new TransactionScope())
                {
                    sut.UpdateShippingCosts(new UpdateShippingCostsRequest
                    {
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
        }

        [TestMethod]
        public void Calculate_Shipping_Costs()
        {
            using (new TransactionScope())
            {
                using (var db = new Entities())
                {
                    var sut = _createSut();
                    var cartService = new CartService();
                    var cart = cartService.CreateCart();

                    var product = db.Products.First(p => p.Id == 1);
                    cartService.AddProduct(cart.Id, product.Id, 17);

                    // Set shipping costs.
                    setShippingCosts(db, "at", product.ShippingCategoryId, 6.75m, 1.5m);
                    setShippingCosts(db, "de", product.ShippingCategoryId, 8m, 2m);

                    // Act.
                    var resultAt = sut.CalculateShippingCosts(cart.Id, "at");
                    var resultDe = sut.CalculateShippingCosts(cart.Id, "de");

                    // Assert
                    Assert.AreEqual(6.75m + 16 * 1.5m, resultAt);
                    Assert.AreEqual(8m + 16 * 2.0m, resultDe);

                    // Add another product with a different shipping category.
                    product = db.Products.First(p => p.ShippingCategoryId != product.ShippingCategoryId);
                    cartService.AddProduct(cart.Id, product.Id, 2);

                    setShippingCosts(db, "at", product.ShippingCategoryId, 0m, 1.1m);
                    setShippingCosts(db, "de", product.ShippingCategoryId, 0m, 2.2m);

                    // Act.
                    resultAt = sut.CalculateShippingCosts(cart.Id, "at");
                    resultDe = sut.CalculateShippingCosts(cart.Id, "de");

                    // Assert
                    Assert.AreEqual((6.75m + 16 * 1.5m) + (1.1m * 2), resultAt);
                    Assert.AreEqual((8m + 16 * 2.0m) + (2.2m * 2), resultDe);
                }
            }
        }

        private void setShippingCosts(Entities db, string countryId, int shippingCategoryId, decimal amount, decimal additionalAmount)
        {
            var costs = db.ShippingCosts.SingleOrDefault(sc => sc.CountryId == countryId && sc.ShippingCategoryId == shippingCategoryId);
            if (costs == null)
            {
                costs = new ShippingCost
                {
                    CountryId = countryId,
                    ShippingCategoryId = shippingCategoryId
                };
            }
            costs.Amount = amount;
            costs.AdditionalAmount = additionalAmount;
            db.SaveChanges();
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
