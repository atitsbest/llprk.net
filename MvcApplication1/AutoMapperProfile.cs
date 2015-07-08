using AutoMapper;
using Llprk.Web.UI.Models;
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
            Mapper.CreateMap<Product, LiquidProduct>()
                .ForMember(d => d.Pictures, o => o.MapFrom(s => s.Pictures
                    .OrderBy(p => p.Pos)
                    .Select(p => new LiquidProductPicture { 
                        Url = p.PictureUrl, 
                        ThumbnailUrl = p.ThumbnailUrl 
                    }).ToArray()));
        }

    }
}