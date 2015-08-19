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
    public class ProductServiceTest : TestBase
    {
        [TestMethod]
        public void Get_product_infos_for_edit()
        {
            var sut = _createSut();

            var result = sut.GetProductForEdit(5);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Update_product()
        {
            var sut = _createSut();
            using (var db = new Entities()) {
                AssertChangedBy(db.Products, () =>
                {
                    // Act
                    sut.UpdateProduct(5, new Llprk.Application.DTOs.Responses.UpdateProductRequest {
                        Available = 10,
                        Description = "descr",
                        IsPublished = true,
                        MetaDescription = "meta",
                        Name = "name",
                        PageTitle = "title",
                        Price = 17.99m,
                        ShippingCategoryId = db.ShippingCategories.First().Id,
                        UrlHandle = "handle"
                    });

                    // Assert
                    var prod = db.Products.Single(p => p.Id == 5);
                    Assert.AreEqual(10, prod.Available);
                    Assert.AreEqual("descr", prod.Description);
                    Assert.AreEqual(true, prod.IsPublished);
                    Assert.AreEqual("meta", prod.MetaDescription);
                    Assert.AreEqual("name", prod.Name);
                    Assert.AreEqual("title", prod.PageTitle);
                    Assert.AreEqual(17.99m, prod.Price);
                    Assert.AreEqual(db.ShippingCategories.First().Id, prod.ShippingCategoryId);
                    Assert.AreEqual("handle", prod.UrlHandle);
                }, 0);
            }
        }

        private ProductService _createSut()
        {
            return new ProductService();
        }
    }
}
