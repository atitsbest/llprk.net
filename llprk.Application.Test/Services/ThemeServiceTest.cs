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
        private string _ThemesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Fixtures\Themes");


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

            var work = theme.Unpublished;
            Assert.AreEqual(1, work.Layouts.Count());
            Assert.AreEqual(1, work.Assets.Count());
            Assert.AreEqual(2, work.Templates.Count());
            Assert.AreEqual(2, work.Snippets.Count());
        }

        [TestMethod]
        public void Create_Unpublished_Theme_if_not_existent()
        {
            try
            {
                var sut = _createSut();

                var theme = sut.GetTheme("theme3");
                var work = theme.Unpublished;

                Assert.AreEqual(theme.Assets.Count(), work.Assets.Count());
                Assert.AreEqual(theme.Snippets.Count(), work.Snippets.Count());
                Assert.AreEqual(theme.Layouts.Count(), work.Layouts.Count());
                Assert.AreEqual(theme.Templates.Count(), work.Templates.Count());
            }
            finally
            {
                Directory.Delete(Path.Combine(_ThemesPath, "theme3", "unpublished"), true);
            }
        }

        [TestMethod]
        public void Publish_Theme()
        {
            var sut = _createSut();

            try
            {
                _directoryCopy(
                    Path.Combine(_ThemesPath, "theme2"),
                    Path.Combine(_ThemesPath, "theme_test"),
                    true);

                // Create copy of theme for publishing
                var theme = sut.GetTheme("theme_test");
                var unpublished = theme.Unpublished;

                // Act
                theme = unpublished.Publish();

                // Assert
                Assert.AreEqual(1, theme.Assets.Count());
            }
            finally
            {
                // Remove cloned theme fixture.
                Directory.Delete(Path.Combine(_ThemesPath, "theme_test"), true);
            }
        }

        [TestMethod]
        public void Update_Item()
        {
            var sut = _createSut();

            var theme = sut.GetTheme("theme2").Unpublished;
            var item = theme.GetItem("snippet1.liquid", "snippets");
            var backup = item.ReadContent();

            try
            {
                item.Update("1234");
                Assert.AreEqual("1234", File.ReadAllText(Path.Combine(_ThemesPath, "theme2", "unpublished", "snippets", "snippet1.liquid")));
            }
            finally
            {
                item.Update(backup);
            }
        }

        /// <summary>
        /// Create Sut.
        /// </summary>
        /// <returns></returns>
        private ThemeService _createSut() {
            return new ThemeService(new Uri(_ThemesPath, UriKind.Absolute));
        }

        /// <summary>
        /// Copy a whole directory.
        /// </summary>
        /// <param name="sourceDirName"></param>
        /// <param name="destDirName"></param>
        /// <param name="copySubDirs"></param>
        private static void _directoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory doesn't exist, create it. 
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location. 
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    _directoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }
}
