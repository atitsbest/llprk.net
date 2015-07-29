using Llprk.Application.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Llprk.Application.DTOs.Requests
{
    public class UpdateShippingCostsRequest
    {
        public class Country
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public ShippingCost[] ShippingCosts { get; set; }
        }

        public class ShippingCost
        {
            public int ShippingCategoryId { get; set; }
            public string ShippingCategoryName { get; set; }
            public decimal Amount { get; set; }
            public decimal AdditionalAmount { get; set; }
        }

        public class ShippingCategory
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public Country[] Countries { get; set; }
    }
}
