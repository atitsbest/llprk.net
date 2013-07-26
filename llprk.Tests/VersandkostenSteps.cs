using Llprk.Web.UI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TechTalk.SpecFlow;

namespace llprk.Tests
{
    [Binding]
    public class VersandkostenSteps
    {
        Order _Order;
        Product _Product;

        [Given(@"Produkt[e]? der Kategorie (.*) werden verschickt")]
        public void AngenommenEinProduktDerKategorieWirdVerschickt(string categoryNames)
        {
            var names = categoryNames.Split(new char[] { ',' });
            _Product = new Product();
            _Product.ShippingCategory = new ShippingCategory() {
                Id = 1,
                Name = "categoryName"
            };
            _Order = new Order();
        }
        
        [Given(@"der Zielland-Code ist (.*)$")]
        public void AngenommenDerZiellandCode(string code)
        {
            _Order.CountryId = code;
        }
        
        [Then(@"sollen die Versandkosten (.*)€ betragen")]
        public void DannSollenDieVersandkostenBetragen(int kosten)
        {
            Assert.AreEqual(kosten, _Order.ShippingCosts); 
        }
    }
}
