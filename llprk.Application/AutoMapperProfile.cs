using AutoMapper;
using Llprk.Application.DTOs.Requests;
using Llprk.Application.DTOs.Responses;
using Llprk.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Llprk.Application
{
    public class AutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<NewPageResponse, Page>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.CreatedAt, o => o.MapFrom(s => DateTime.UtcNow));

            Mapper.CreateMap<EditPageResponse, Page>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.CreatedAt, o => o.Ignore());


            Mapper.CreateMap<Page, EditPageRequest>();

            Mapper.CreateMap<Product, EditProductResponse>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id));
            Mapper.CreateMap<Picture, EditProductResponse.PictureResponse>()
                .ForMember(d => d.Url, o => o.MapFrom(s => s.ThumbnailUrl));

            Mapper.CreateMap<UpdateProductRequest, Product>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.CategoryId, o => o.Ignore())
                .ForMember(d => d.Category, o => o.Ignore())
                .ForMember(d => d.ShippingCategory, o => o.Ignore())
                .ForMember(d => d.ChargeTaxes, o => o.Ignore())
                .ForMember(d => d.Pictures, o => o.Ignore())
                .ForMember(d => d.Tags, o => o.Ignore())
                .ForMember(d => d.OrderLines, o => o.Ignore())
                .ForMember(d => d.LineItems, o => o.Ignore())
                .ForMember(d => d.CreatedAt, o => o.Ignore());
        }

    }
}