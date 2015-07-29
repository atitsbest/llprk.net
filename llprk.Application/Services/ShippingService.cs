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
    }

    public class ShippingService : IShippingService
    {
        public void UpdateShippingCategory(int id, string name)
        {
            var db = new Entities();
            var category = db.ShippingCategories.Single(c => c.Id == id);
            category.Name = name;
            db.Entry(category).State = EntityState.Modified;
            db.SaveChanges();
        }

        public ShippingCategory CreateShippingCategory(string name)
        {
            var db = new Entities();
            var cat = new ShippingCategory
            {
                Name = name
            };

            db.ShippingCategories.Add(cat);
            db.SaveChanges();

            return cat;
        }


        public void DeleteShippingCost(int id)
        {
            try
            {
                var db = new Entities();
                var cat = db.ShippingCategories.Single(c => c.Id == id);
                db.ShippingCategories.Remove(cat);
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                throw new ApplicationException("Shipping category is in use. Cannot delete it.");
            }
        }


        public void UpdateShippingCosts(UpdateShippingCostsRequest data)
        {
            var db = new Entities();

            foreach (var country in data.Countries)
            {
                foreach (var cost in country.ShippingCosts) {
                    var shippingCost = db.ShippingCosts.SingleOrDefault(sc => sc.CountryId == country.Id && sc.ShippingCategoryId == cost.ShippingCategoryId);
                    if (shippingCost != null)
                    {
                        shippingCost.Amount = cost.Amount;
                        shippingCost.AdditionalAmount = cost.AdditionalAmount;
                        db.Entry(shippingCost).State = EntityState.Modified;
                    }
                    else
                    {
                        shippingCost = new ShippingCost {
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
}
