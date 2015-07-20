using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Llprk.DataAccess.Models
{
    /// <summary>
    /// Domain Class für ein Theme. Enthält alle Funktion und Information
    /// zum Thema "Theme".
    /// </summary>
    public class Theme
    {
        public class Item {
            public string Name;
            public string Type;
        }

        private string _Root;

        public string Name { get; set; }

        public IEnumerable<Item> Layouts { get; private set; }
        public IEnumerable<Item> Assets { get; private set; }
        public IEnumerable<Item> Templates { get; private set; }
        public IEnumerable<Item> Snippets { get; private set; }

        /// <summary>
        /// Get unpublished version of the Theme.
        /// </summary>
        public Theme UnPublishedVersion
        {
            get
            {
                return _UnPublishedVersion = _UnPublishedVersion
                    ?? new Theme(new Uri(Path.Combine(_Root, @"..\_unpublished")));
            }
        }
        private Theme _UnPublishedVersion;


        /// <summary>
        /// CTR
        /// </summary>
        /// <param name="root"></param>
        public Theme(Uri root)
        {
            if (root == null) throw new ArgumentNullException("root");
            if (!root.IsFile) throw new ArgumentException("Root must be a path name.");

            _Root = root.AbsolutePath;
            _Initialize();
        }


        /// <summary>
        /// Get all the Items of a Theme type (i.e.: Assets, Templates, ...)
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private IEnumerable<Theme.Item> _EnumerateItems(string path, string type)
        {
            var typePath = _isUnpublished(path)
                ? Path.Combine(path, type)
                : Path.Combine(path, "_current", type);
            return Directory.EnumerateFiles(typePath).Select(fi => new Theme.Item { Name = fi, Type = "layout" });
        }

        /// <summary>
        /// Loads alls the Theme-Information.
        /// </summary>
        private void _Initialize()
        {
            if (!Directory.Exists(_Root)) throw new ArgumentException(string.Format("Cannot find theme '{0}'.", Path.GetFileName(_Root)));

            Name = Path.GetFileName(_Root);
            Layouts = _EnumerateItems(_Root, "layouts");
            Assets = _EnumerateItems(_Root, "assets");
            Templates = _EnumerateItems(_Root, "templates");
            Snippets = _EnumerateItems(_Root, "snippets");
        }

        /// <summary>
        /// Is this the path of an unpublished theme?
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static bool _isUnpublished(string path)
        {
            return "_unpublished" == Path.GetFileName(path);
        }
    }
}
