using Llprk.Application.DTOs.Requests;
using Llprk.Web.UI.Areas.Admin.Models;
using Llprk.Web.UI.Controllers;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Llprk.Web.UI.Areas.Admin.Controllers
{
    [Authorize]
    public partial class ThemesController : ApplicationController
    {
        /// <summary>
        /// CTR
        /// </summary>
        public ThemesController()
        {
        }

        //
        // GET: /Themes/
        public virtual ActionResult Index()
        {
            var viewModel = new ThemesRequest
            {
                Themes = Directory.EnumerateDirectories(Server.MapPath("~/Themes/")).Select(di =>
                    new ThemesRequest.Theme
                    {
                        Name = Path.GetFileName(di),
                        IsActive = false
                    })
            };

            return View(viewModel);
        }

        //
        // GET: /Themes/Edit/{id}
        public virtual ActionResult Edit(string id)
        {
            var templatePath = Server.MapPath("~/Themes/" + id);
            var viewModel = new EditThemeRequest(id)
            {
                Layouts = Directory.EnumerateFiles(Path.Combine(templatePath, "layouts")).Select(fi => new EditThemeRequest.Item(fi, "layout")),
                Assets = Directory.EnumerateFiles(Path.Combine(templatePath, "assets")).Select(fi => new EditThemeRequest.Item(fi, "asset")),
                Templates = Directory.EnumerateFiles(Path.Combine(templatePath, "templates")).Select(fi => new EditThemeRequest.Item(fi, "template")),
                Snippets = Directory.EnumerateFiles(Path.Combine(templatePath, "snippets")).Select(fi => new EditThemeRequest.Item(fi, "snippet"))
            };

            return View(viewModel);
        }

        //
        // GET: /Themes/Edit/{id}
        public virtual ActionResult Content(string id, string type, string theme)
        {
            var path = Path.Combine(Server.MapPath("~/Themes/"), theme, type+"s", id);
            var mimeType = MimeTypes.MimeTypeMap.GetMimeType(Path.GetExtension(path));
            var mimeGroup = "unsupported";
            var viewModel = new ThemeContent
            {
                Id = id,
                Type = type,
                Theme = theme,
                ContentType = mimeType
            };

            // Richtigen Editor auswählen...
            if (System.IO.File.Exists(path))
            {
                if (mimeType.StartsWith("image/"))
                {
                    mimeGroup = "image";
                }
                else if (new string[] { "text/css", "application/x-javascript", "application/x-liquid-template" }.Contains(mimeType))
                {
                    mimeGroup = "text";
                    viewModel.Content = System.IO.File.ReadAllText(path);
                }
            }
            else
            {
                mimeGroup = "404";
            }

            return View(string.Format("Editors/_{0}", mimeGroup), viewModel);
        }
    }
}