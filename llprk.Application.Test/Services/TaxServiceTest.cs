using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Llprk.DataAccess.Models;
using Llprk.Application.Services;

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

        private TaxService _createSut()
        {
            return new TaxService();
        }
    }
}
