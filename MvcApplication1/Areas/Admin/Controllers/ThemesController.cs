using Llprk.Application.DTOs.Requests;
using Llprk.Application.Services;
using Llprk.DataAccess.Models.Theme;
using Llprk.Web.UI.Areas.Admin.Models;
using Llprk.Web.UI.Controllers;
using Llprk.Web.UI.Controllers.Results;
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

        /// <summary>
        /// See the active and all inactive themes.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Edit unpublished theme.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual ActionResult Edit(string id)
        {
            var theme = _ThemeService.GetTheme(id);
            var viewModel = new EditThemeRequest(id)
            {
                //Items = theme.UnpublishedItems.Keys
            };

            return View(viewModel);
        }

        /// <summary>
        /// Updates the content of the unpublished item.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="theme"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public virtual ActionResult UpdateItem(string id, string type, string theme, string content)
        {
            var themeInst = _ThemeService.GetTheme(theme);
            var item = themeInst.GetUnpublishedItem(id, type);

            item.WriteContent(content);

            return new EntityResult(string.Format("{0} saved.", id));
        }

        /// <summary>
        /// Returns the content of the unpublished item.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="theme"></param>
        /// <returns></returns>
        public virtual ActionResult Content(string id, string type, string theme)
        {
            var mimeGroup = "unsupported";
            var themeInst = _ThemeService.GetTheme(theme);

            IThemeItem item = themeInst.GetUnpublishedItem(id, type);

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