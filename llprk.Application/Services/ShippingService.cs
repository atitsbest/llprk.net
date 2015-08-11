using AutoMapper;
using Llprk.Application.DTOs.Requests;
using Llprk.Application.DTOs.Responses;
using Llprk.DataAccess.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Llprk.Application.Services
{
    public interface IShippingService
    {
        void UpdateShippingCategory(int id, string name);
        ShippingCategory CreateShippingCategory(string name);

        void DeleteShippingCost(int id);

        void UpdateShippingCosts(UpdateShippingCostsRequest data);

        decimal CalculateShippingCosts(int cartId, string country);
    }

    public class ShippingService : IShippingService
    {
        public void UpdateShippingCategory(int id, string name)
        {
            using (var db = new Entities())
            {
                var category = db.ShippingCategories.Single(c => c.Id == id);
                category.Name = name;
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public ShippingCategory CreateShippingCategory(string name)
        {
            using (var db = new Entities())
            {
                var cat = new ShippingCategory
                {
                    Name = name
                };

                db.ShippingCategories.Add(cat);
                db.SaveChanges();

                return cat;
            }
        }


        public void DeleteShippingCost(int id)
        {
            try
            {
                using (var db = new Entities())
                {
                    var cat = db.ShippingCategories.Single(c => c.Id == id);
                    db.ShippingCategories.Remove(cat);
                    db.SaveChanges();
                }
            }
            catch (DbUpdateException)
            {
                throw new ApplicationException("Shipping category is in use. Cannot delete it.");
            }
        }


        public void UpdateShippingCosts(UpdateShippingCostsRequest data)
        {
            using (var db = new Entities())
            {

                foreach (var country in data.Countries)
                {
                    foreach (var cost in country.ShippingCosts)
                    {
                        var shippingCost = db.ShippingCosts.SingleOrDefault(sc => sc.CountryId == country.Id && sc.ShippingCategoryId == cost.ShippingCategoryId);
                        if (shippingCost != null)
                        {
                            shippingCost.Amount = cost.Amount;
                            shippingCost.AdditionalAmount = cost.AdditionalAmount;
                            db.Entry(shippingCost).State = EntityState.Modified;
                        }
                        else
                        {
                            shippingCost = new ShippingCost
                            {
                                CountryId = country.Id,
                                ShippingCategoryId = cost.ShippingCategoryId,
                                Amount = cost.Amount,
                                AdditionalAmount = cost.AdditionalAmount
                            };

                            db.ShippingCosts.Add(shippingCost);
                        }
                    }
                }

                db.SaveChanges();
            }
        }

        /// <summary>
        /// Und so wird's berechnet:
        ///  * Die höchste Basis-Rate wird ermittelt.
        ///  * für alle wieter Produkte wir die AdditionalAmount addiert.
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="country"></param>
        /// <returns></returns>
        public decimal CalculateShippingCosts(int cartId, string countryCode)
        {
            using (var db = new Entities())
            {
                var cart = db.Carts
                    .Include(x => x.LineItems.Select(l => l.Product.ShippingCategory.ShippingCosts))
                    .Single(c => c.Id == cartId);

                var highest = cart.LineItems.Max(l =>
                {
                    if (l.Product.ShippingCategory == null) throw new ApplicationException("Product has no shipping category. Cannot calculate shipping costs!");
                    var cost = l.Product.ShippingCategory.ShippingCosts.Single(sc => sc.CountryId == countryCode);
                    return cost.Amount - cost.AdditionalAmount;
                });
                var additional = cart.LineItems.Sum(l => l.Product.ShippingCategory.ShippingCosts.Single(sc => sc.CountryId == countryCode).AdditionalAmount * l.Qty);

                return highest + additional;
            }
        }
    }
}
