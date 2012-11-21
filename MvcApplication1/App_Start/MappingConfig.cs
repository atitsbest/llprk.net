using AutoMapper;
using MvcApplication1.Models;
using MvcApplication1.ViewModels;

namespace MvcApplication1
{
    public class MappingConfig
    {
        public static void RegisterMappings()
        {
            Mapper.CreateMap<Product, ProductEdit>()
                .ForMember(d => d.AllPictures, o => o.Ignore());
        }
    }
}