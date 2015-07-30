using AutoMapper;
using Llprk.Application.DTOs.Requests;
using Llprk.Application.DTOs.Responses;
using Llprk.DataAccess.Models;
using System;
using System.Data.Entity;
using System.Linq;

namespace Llprk.Application.Services
{
    public interface ITaxService
    {
        void UpdateCountryTax(string country, int percent);

        decimal TaxForCountry(int cartId, string country);
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
            using (var db = new Entities())
            {
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
                else
                {
                    tax.Percent = percent;
                }

                db.SaveChanges();
            }
        }

        public decimal TaxForCountry(int cartId, string country)
        {
            using (var db = new Entities())
            {
                var cart = db.Carts
                    .Include(i => i.LineItems.Select(l => l.Product))
                    .Single(c => c.Id == cartId);
                var cnt = db.Countries
                    .Include(i => i.Taxes)
                    .Single(c => c.Id == country);

                return cart.LineItems.Sum(li =>
                {
                    if (li.Product.ChargeTaxes)
                    {
                        return li.Subtotal - (li.Subtotal / ((100.0m + cnt.Taxes.First().Percent) / 100.0m));
                    }
                    else
                    {
                        return 0;
                    }
                });
            }
        }
    }
}
