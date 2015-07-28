using AutoMapper;
using Llprk.Application.DTOs.Requests;
using Llprk.Application.DTOs.Responses;
using Llprk.DataAccess.Models;
using System;
using System.Linq;

namespace Llprk.Application.Services
{
    public interface ITaxService
    {
        void UpdateCountryTax(string country, int percent);
    }

    public class TaxService : ITaxService
    {
        /// <summary>
        /// Update the tax for a single country.
        /// </summary>
        /// <param name="country"></param>
        /// <param name="percent"></param>
        public void UpdateCountryTax(string country, int percent)
        {
            var db = new Entities();
            var tax = db.Taxes.SingleOrDefault(t => t.CountryId == country);

            if (tax == null)
            {
                tax = new Tax
                {
                    CountryId = country,
                    Percent = percent
                };
                db.Taxes.Add(tax);
            }
            else {
                tax.Percent = percent;
            }

            db.SaveChanges();
        }
    }
}
