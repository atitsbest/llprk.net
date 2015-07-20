using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Llprk.Application.Services;
using System.IO;

namespace llprk.Application.Test.Services
{
    [TestClass]
    public class ThemeServiceTest
    {
        [TestMethod]
        public void Get_List_of_all_Themes()
        {
            var sut = _createSut();

            var themes = sut.GetAllThemes();

            Assert.AreEqual(3, themes.Count());

        }

        [TestMethod]
        public void Get_Information_about_all_Themes()
        {
            var sut = _createSut();

            var themes = sut.GetAllThemes();

            var names = themes.Select(t => t.Name);
            Assert.IsTrue(names.Contains("minimal"));
            Assert.IsTrue(names.Contains("theme2"));
            Assert.IsTrue(names.Contains("theme3"));
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void Check_if_Theme_exists()
        {
            var sut = _createSut();

            sut.GetTheme("gibtsgarnicht");
        }

        [TestMethod]
        public void Get_Theme_Items()
        {
            var sut = _createSut();

            var theme = sut.GetTheme("theme2");

            Assert.AreEqual(1, theme.Layouts.Count());
            Assert.AreEqual(3, theme.Assets.Count());
            Assert.AreEqual(2, theme.Templates.Count());
            Assert.AreEqual(2, theme.Snippets.Count());
        }


        [TestMethod]
        public void Get_Unpublished_Version_of_Theme()
        {
            var sut = _createSut();

            var theme = sut.GetTheme("theme2");

            var work = theme.UnPublishedVersion;
            Assert.AreEqual(1, work.Layouts.Count());
            Assert.AreEqual(3, work.Assets.Count());
            Assert.AreEqual(2, work.Templates.Count());
            Assert.AreEqual(2, work.Snippets.Count());
        }

        /// <summary>
        /// Create Sut.
        /// </summary>
        /// <returns></returns>
        private ThemeService _createSut() {
            var themesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Fixtures\Themes");
            return new ThemeService(new Uri(themesPath, UriKind.Absolute));
        }
    }
}
