using MimeTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Llprk.DataAccess.Models.Theme
{
    /// <summary>
    /// Domain Class für ein Theme. Enthält alle Funktion und Information
    /// zum Thema "Theme".
    /// </summary>
    public class FileBasedTheme : ITheme
    {
        public class Item : IThemeItem {
            public string Name { get; set; }
            public string AbsolutePath { get; set; }
            public string Type { get; set; }
            public string ContentType { get; set; }

            public string ReadContent() {
                return File.ReadAllText(AbsolutePath);
            }
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
                    ?? new UnpublishedFileBasedTheme(new Uri(Path.Combine(_Root, @"unpublished")), Name);
            }
        }
        private UnpublishedFileBasedTheme _UnPublishedVersion;


        /// <summary>
        /// CTR
        /// </summary>
        protected FileBasedTheme()
        { }

        /// <summary>
        /// CTR
        /// </summary>
        /// <param name="root"></param>
        public FileBasedTheme(Uri root, string name = null)
        {
            if (root == null) throw new ArgumentNullException("root");
            if (!root.IsFile) throw new ArgumentException("Root must be a path name.");

            _Root = root.AbsolutePath;
            _Initialize(name);
        }

        /// <summary>
        /// Returns a single Item if found.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IThemeItem GetItem(string name, string type)
        {
            IThemeItem result = null;

            result = result ?? Assets.SingleOrDefault(x => x.Name == name && x.Type == type);
            result = result ?? Layouts.SingleOrDefault(x => x.Name == name && x.Type == type);
            result = result ?? Templates.SingleOrDefault(x => x.Name == name && x.Type == type);
            result = result ?? Snippets.SingleOrDefault(x => x.Name == name && x.Type == type);

            return result;
        }

        /// <summary>
        /// Get all the Items of a Theme type (i.e.: Assets, Templates, ...)
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private FileBasedTheme.Item[] _EnumerateItems(string path, string type)
        {
            var typePath = _isUnpublished(path)
                ? Path.Combine(path, type)
                : Path.Combine(path, "current", type);
            return Directory.EnumerateFiles(typePath).Select(fi => new FileBasedTheme.Item { 
                Name = Path.GetFileName(fi),
                AbsolutePath = fi,
                Type = type,
                ContentType = MimeTypeMap.GetMimeType(Path.GetExtension(fi))
            }).ToArray();
        }

        /// <summary>
        /// Loads alls the Theme-Information.
        /// </summary>
        protected void _Initialize(string name = null)
        {
            if (!Directory.Exists(_Root)) throw new ArgumentException(string.Format("Cannot find {0} of theme '{1}'.", Path.GetFileName(_Root), name));

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
    class UnpublishedFileBasedTheme : FileBasedTheme, IUnpublishedTheme
    {
        /// <summary>
        /// Semaphore to regulate theme access, while publishing the theme.
        /// </summary>
        protected static string _PublishLock = "lock";

        /// <summary>
        /// CTR
        /// </summary>
        /// <param name="root"></param>
        public UnpublishedFileBasedTheme(Uri root, string name = null)
        {
            // Copy current version, if unpublished doesnt exist.
            if (!Directory.Exists(root.AbsolutePath))
            {
                var currentPath = Path.Combine(root.AbsolutePath, @"..\current");
                _copyDirectory(currentPath, root.AbsolutePath, true);
            }

            _Root = root.AbsolutePath;
            _Initialize(name);
        }

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

                return new FileBasedTheme(new Uri(Path.Combine(_Root, @"..\")));
            }
        }

        /// <summary>
        /// Copy a whole directory.
        /// </summary>
        /// <param name="sourceDirName"></param>
        /// <param name="destDirName"></param>
        /// <param name="copySubDirs"></param>
        private static void _copyDirectory(string sourceDirName, string destDirName, bool copySubDirs)
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
                    _copyDirectory(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

    }
}
