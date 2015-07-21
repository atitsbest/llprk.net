using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Llprk.DataAccess.Models
{
    /// <summary>
    /// So soll ein Thema funktionieren:
    /// Es gibt das aktuelle (publizierte) Thema und dann noch ein
    /// Thema, dass in Arbeit ist, oder anders: nicht-publiziert.
    /// 
    /// Sobald ein Thema bearbeitet wird, wird mit der nicht-publizierten 
    /// Variante gearbeitet.
    /// </summary>
    public interface ITheme
    {

        string Name { get; set; }
        IThemeItem[] Assets { get; }
        IThemeItem[] Layouts { get; }
        IThemeItem[] Snippets { get; }
        IThemeItem[] Templates { get; }

        IUnpublishedTheme Unpublished { get; }
    }

    public interface IUnpublishedTheme : ITheme
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>An instance of the published theme</returns>
        ITheme Publish();
    }

    public interface IThemeItem
    {
        string Name { get; set; }
        string Type { get; set; }
    }

    /// <summary>
    /// Domain Class für ein Theme. Enthält alle Funktion und Information
    /// zum Thema "Theme".
    /// </summary>
    public class FileSystemBasedTheme : ITheme
    {
        public class Item : IThemeItem {
            public string Name { get; set; }
            public string Type { get; set; }
        }

        protected string _Root;

        public string Name { get; set; }

        public IThemeItem[] Layouts { get; private set; }
        public IThemeItem[] Assets { get; private set; }
        public IThemeItem[] Templates { get; private set; }
        public IThemeItem[] Snippets { get; private set; }

        /// <summary>
        /// Get unpublished version of the Theme.
        /// </summary>
        public IUnpublishedTheme Unpublished
        {
            get
            {
                return _UnPublishedVersion = _UnPublishedVersion
                    ?? new UnpublishedFileSystemBasedTheme(new Uri(Path.Combine(_Root, @"unpublished")), Name);
            }
        }
        private UnpublishedFileSystemBasedTheme _UnPublishedVersion;


        /// <summary>
        /// CTR
        /// </summary>
        /// <param name="root"></param>
        public FileSystemBasedTheme(Uri root, string name = null)
        {
            if (root == null) throw new ArgumentNullException("root");
            if (!root.IsFile) throw new ArgumentException("Root must be a path name.");

            _Root = root.AbsolutePath;
            _Initialize(name);
        }


        /// <summary>
        /// Get all the Items of a Theme type (i.e.: Assets, Templates, ...)
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private FileSystemBasedTheme.Item[] _EnumerateItems(string path, string type)
        {
            var typePath = _isUnpublished(path)
                ? Path.Combine(path, type)
                : Path.Combine(path, "current", type);
            return Directory.EnumerateFiles(typePath).Select(fi => new FileSystemBasedTheme.Item { Name = fi, Type = "layout" }).ToArray();
        }

        /// <summary>
        /// Loads alls the Theme-Information.
        /// </summary>
        private void _Initialize(string name = null)
        {
            if (!Directory.Exists(_Root)) throw new ArgumentException(string.Format("Cannot find theme '{0}'.", Path.GetFileName(_Root)));

            Name = name ?? Path.GetFileName(_Root);
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
            return "unpublished" == Path.GetFileName(path);
        }
    }

    /// <summary>
    /// An unpublished Theme. Can be published.
    /// </summary>
    class UnpublishedFileSystemBasedTheme : FileSystemBasedTheme, IUnpublishedTheme
    {
        /// <summary>
        /// Semaphore to regulate theme access, while publishing the theme.
        /// </summary>
        protected static string _PublishLock = "lock";

        /// <summary>
        /// CTR
        /// </summary>
        /// <param name="root"></param>
        public UnpublishedFileSystemBasedTheme(Uri root, string name = null)
            : base(root, name)
        { }

        public ITheme Publish()
        {
            lock (_PublishLock)
            {
                var currentPath = Path.Combine(_Root, @"..\current");
                var currentTmpPath = Path.Combine(_Root, @"..\current_tmp");

                // Swap current and unpublished ...
                // ...and then delete the old current.
                Directory.Move(currentPath, currentTmpPath);
                Directory.Move(_Root, currentPath);
                Directory.Delete(currentTmpPath, true);

                return new FileSystemBasedTheme(new Uri(Path.Combine(_Root, @"..\")));
            }
        }
    }
}
