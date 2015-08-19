using System.Linq;
using AutoMapper;
using Llprk.DataAccess.Models;
using Llprk.Web.UI.Areas.Admin.Models;

namespace Llprk.Web.UI
{
    public class MappingConfig
    {
        public static void RegisterMappings()
        {
            Mapper.CreateMap<Parameter, ParameterIndex>();

            Mapper.CreateMap<PostOrderNew, OrderNew>();
        }
    }
}