using AutoMapper;
using Llprk.DataAccess.Models;
using Llprk.Web.UI.Areas.Admin.Models;
using Llprk.Web.UI.Areas.Store.Models;
using Llprk.Web.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Llprk.Web.UI
{
    public class AutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            /* ======================================================
             * STORE
             * ====================================================== */

            Mapper.CreateMap<Product, LiquidProduct>()
                .ForMember(d => d.Pictures, o => o.MapFrom(s => s.Pictures
                    .OrderBy(p => p.Pos)
                    .Select(p => new LiquidProductPicture { 
                        Url = p.PictureUrl, 
                        ThumbnailUrl = p.ThumbnailUrl 
                    }).ToArray()));

            Mapper.CreateMap<LineItem, LiquidLineItem>()
                .ForMember(d => d.Product, o => o.MapFrom(s => Mapper.Map<LiquidProduct>(s.Product)));


            // CheckoutIndex
            Mapper.CreateMap<LineItem, CheckoutIndex.LineItem>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Product.Name));

            Mapper.CreateMap<Country, CheckoutIndex.Country>();

            Mapper.CreateMap<Cart, CheckoutIndex>()
                .ForMember(d => d.LineItems, o => o.MapFrom(s => Mapper.Map<IEnumerable<CheckoutIndex.LineItem>>(s.LineItems)))
                .ForMember(d => d.Countries, o => o.Ignore());

            Mapper.CreateMap<Country, TaxIndex.Country>()
                .ForMember(d => d.TaxId, o => o.MapFrom(s => s.Taxes.Any() ? s.Taxes.First().Id : -1))
                .ForMember(d => d.TaxPercent, o => o.MapFrom(s => s.Taxes.Any() ? s.Taxes.First().Percent : 0));

            Mapper.CreateMap<Country, ShippingCostIndex.Country>();
        }

    }
}