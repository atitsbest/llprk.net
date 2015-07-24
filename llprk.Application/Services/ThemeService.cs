using Llprk.DataAccess.Models;
using Llprk.DataAccess.Models.Theme;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Llprk.Application.Services
{
    public class ThemeService
    {
        /// <summary>
        /// Contains the Root-Path to the Themes.
        /// </summary>
        private Uri _Root;

        /// <summary>
        /// CTR
        /// </summary>
        /// <param name="root"></param>
        public ThemeService(Uri root)
        {
            if (root == null) throw new ArgumentNullException("root");
            if (!root.IsFile) throw new ArgumentException("Root must be a path name.");

            _Root = root;
        }

        /// <summary>
        /// Get Quid Information to all available Themes.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ITheme> GetAllThemes()
        {
            return Directory.EnumerateDirectories(_Root.AbsolutePath).Select(path =>
                new FileBasedTheme(new Uri(path))
            );
        }

        /// <summary>
        /// Get a single Theme, with all functions and infos.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ITheme GetTheme(string name)
        {
            var themePath = Path.Combine(_Root.AbsolutePath, name);
            if (!Directory.Exists(themePath)) throw new ArgumentException(string.Format("Cannot find theme '{0}'.", name));

            return new FileBasedTheme(new Uri(themePath));
        }

    }
}
