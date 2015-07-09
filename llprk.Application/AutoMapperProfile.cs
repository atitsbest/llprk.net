using AutoMapper;
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
                .ForMember(d => d.Id, o => o.Ignore());
        }

    }
}