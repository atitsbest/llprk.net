using SimpleInjector.Packaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Llprk.Web.UI
{
    public class DependencyInjectionConfig : IPackage
    {
        public void RegisterServices(SimpleInjector.Container container)
        {
            //container.Register<ILehrerRepository, LehrerRepository>();
            container.RegisterSingle<Uri>(new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\Themes")));
        }
    }
}
