using Llprk.Application.Services;
using SimpleInjector.Packaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Llprk.Application
{
    public class DependencyInjectionConfig : IPackage
    {
        public void RegisterServices(SimpleInjector.Container container)
        {
            container.Register<ThemeService>();
        }
    }
}

