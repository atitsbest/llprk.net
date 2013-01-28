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
                .ForMember(d => d.AllPictures, o => o.Ignore());
        }
    }
}