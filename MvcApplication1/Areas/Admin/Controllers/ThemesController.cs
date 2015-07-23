using Llprk.Application.DTOs.Requests;
using Llprk.Application.Services;
using Llprk.DataAccess.Models.Theme;
using Llprk.Web.UI.Areas.Admin.Models;
using Llprk.Web.UI.Controllers;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Llprk.Web.UI.Areas.Admin.Controllers
{
    [Authorize]
    public partial class ThemesController : ApplicationController
    {
        private ThemeService _ThemeService;

        /// <summary>
        /// CTR
        /// </summary>
        public ThemesController(ThemeService themes)
        {
            if (themes == null) throw new ArgumentNullException("themes");

            _ThemeService = themes;
        }

        //
        // GET: /Themes/
        public virtual ActionResult Index()
        {
            var viewModel = new ThemesRequest
            {
                Themes = _ThemeService.GetAllThemes().Select(t =>
                    new ThemesRequest.Theme
                    {
                        Name = t.Name,
                        IsActive = false
                    })
            };

            return View(viewModel);
        }

        //
        // GET: /Themes/Edit/{id}
        public virtual ActionResult Edit(string id)
        {
            var theme = _ThemeService.GetTheme(id).Unpublished;
            var viewModel = new EditThemeRequest(id)
            {
                Layouts = theme.Layouts.Select(i => new EditThemeRequest.Item(i.Name, i.Type)),
                Assets = theme.Assets.Select(i => new EditThemeRequest.Item(i.Name, i.Type)),
                Snippets = theme.Snippets.Select(i => new EditThemeRequest.Item(i.Name, i.Type)),
                Templates = theme.Templates.Select(i => new EditThemeRequest.Item(i.Name, i.Type)),
            };

            return View(viewModel);
        }

        //
        // GET: /Themes/Edit/{id}
        public virtual ActionResult Content(string id, string type, string theme)
        {
            var mimeGroup = "unsupported";
            var themeInst = _ThemeService.GetTheme(theme).Unpublished;

            IThemeItem item = themeInst.GetItem(id, type);


            var viewModel = new ThemeContent
            {
                Id = id,
                Type = type,
                Theme = theme,
                ContentType = item == null ? "" : item.ContentType
            };

            // Richtigen Editor auswählen...
            if (item != null)
            {
                if (item.ContentType.StartsWith("image/"))
                {
                    mimeGroup = "image";
                }
                else if (new string[] { "text/css", "application/x-javascript", "application/x-liquid-template" }.Contains(item.ContentType))
                {
                    mimeGroup = "text";
                    viewModel.Content = item.ReadContent();
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