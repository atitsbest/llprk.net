using System.Linq;
using AutoMapper;
using Llprk.Web.UI.Models;
using Llprk.Web.UI.ViewModels;

namespace Llprk.Web.UI
{
    public class MappingConfig
    {
        public static void RegisterMappings()
        {
            Mapper.CreateMap<Product, ProductEdit>()
                .ForMember(d => d.Pictures, o => o.MapFrom(s => s.OrderedPictures))
                .ForMember(d => d.AllPictures, o => o.Ignore());

            Mapper.CreateMap<Parameter, ParameterIndex>();

            Mapper.CreateMap<PostOrderNew, OrderNew>();
        }
    }
}