using System.Web;
using System.Web.Mvc;
using TypeLite;

namespace Llprk.Web.UI.Areas.Admin.Models
{
    [TsClass(Module = "DTOs.Tax")]
    public class TaxIndex
    {
        [TsClass(Module = "DTOs.Tax")]
        public class Country
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public int TaxId { get; set; }
            public int TaxPercent { get; set; }
        }

        public Country[] Countries { get; set; }

        // Urls
        public string ChangeCountryTaxUrl { get; set; }

        /// <summary>
        /// CTR
        /// </summary>
        public TaxIndex()
        {
            var url = new UrlHelper(HttpContext.Current.Request.RequestContext);
            ChangeCountryTaxUrl = url.Action(MVC.Admin.Taxes.Update());
        }
    }
}