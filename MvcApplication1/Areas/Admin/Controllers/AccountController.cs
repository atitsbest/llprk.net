using Llprk.Web.UI.Areas.Store.Models;
using System.Web.Mvc;
using System.Web.Security;

namespace Llprk.Web.UI.Areas.Admin.Controllers
{
    [Authorize]
    public partial class AccountController : Controller
    {
        //
        // GET: /Account/Login

        [AllowAnonymous]
        public virtual ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public virtual ActionResult Login(LoginModel model, string returnUrl)
        {
            if (FormsAuthentication.Authenticate(model.UserName, model.Password))
            {
                FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                return Redirect(FormsAuthentication.DefaultUrl);
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        //
        // GET: /Account/LogOff

        //[ValidateAntiForgeryToken]
        public virtual ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return Redirect(FormsAuthentication.LoginUrl);
        }
    }
}
