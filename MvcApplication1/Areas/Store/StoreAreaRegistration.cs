using System.Web.Mvc;

namespace Llprk.Web.UI.Areas.Store
{
    public class StoreAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Store";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Store_default",
                "store/{controller}/{action}/{id}",
                new { controller="Shop", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
