using MimeTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public class Item : IThemeItem
        {
            public string Name { get; protected set; }
            public string AbsolutePath { get; private set; }
            public string Type { get; private set; }
            public string ContentType { get; private set; }

            /// <summary>
            /// CTR
            /// </summary>
            /// <param name="name"></param>
            /// <param name="type"></param>
            /// <param name="absolutePath"></param>
            public Item(string name, string type, string absolutePath)
            {
                Name = name;
                Type = type;
                AbsolutePath = absolutePath;
                ContentType = MimeTypeMap.GetMimeType(Path.GetExtension(name));
            }

            /// <summary>
            /// Get Item-Content.
            /// </summary>
            /// <returns></returns>
            public string ReadContent()
            {
                return File.ReadAllText(AbsolutePath);
            }
        }

        /// <summary>
        /// The unpublished version of an theme-item.
        /// Has the ability to change (rename, update).
        /// </summary>
        public class UnpublishedItem : Item, IUnpublishedThemeItem
        {
            /// <summary>
            /// CTR
            /// </summary>
            /// <param name="name"></param>
            /// <param name="type"></param>
            /// <param name="absolutePath"></param>
            public UnpublishedItem(string name, string type, string absolutePath)
                : base(name, type, absolutePath)
            { }
            /// <summary>
            /// Updates the files content.
            /// </summary>
            /// <param name="content"></param>
            public void WriteContent(string content)
            {
                File.WriteAllText(AbsolutePath, content);
            }

            /// <summary>
            /// Renames the file.
            /// </summary>
            /// <param name="newName"></param>
            public void Rename(string newName)
            {
                var newPath = Path.Combine(Path.GetFullPath(AbsolutePath), newName);
                File.Move(AbsolutePath, newPath);
                Name = newName;
            }
        }

        /// <summary>
        /// Semaphore to regulate theme access, while publishing the theme.
        /// </summary>
        protected static string _PublishLock = "lock";

        /// <summary>
        /// Root of the Theme (without 'current' or 'unpublished')
        /// </summary>
        protected string _Root;
        protected string _CurrentRoot;
        protected string _UnpublishedRoot;

        public string Name { get; private set; }

        public IThemeItem[] Layouts { get { return _Items["layouts"].Values.ToArray(); } }
        public IThemeItem[] Assets { get { return _Items["assets"].Values.ToArray(); } }
        public IThemeItem[] Templates { get { return _Items["templates"].Values.ToArray(); } }
        public IThemeItem[] Snippets { get { return _Items["snippets"].Values.ToArray(); } }

        /// <summary>
        /// All the unpublished items.
        /// </summary>
        public Dictionary<string, Dictionary<string, IUnpublishedThemeItem>> UnpublishedItems
        {
            get
            {
                return _UnpublishedItems.ToDictionary(
                    k => k.Key,
                    v => v.Value.ToDictionary(
                        k1 => k1.Key,
                        v1 => (IUnpublishedThemeItem)v1.Value));
            }
        }

        /// <summary>
        /// Contains ALL items.
        /// Key: type/name
        /// </summary>
        protected Dictionary<string, Dictionary<string, Item>> _Items;
        protected Dictionary<string, Dictionary<string, UnpublishedItem>> _UnpublishedItems;

        /// <summary>
        /// CTR
        /// </summary>
        /// <param name="root"></param>
        public FileBasedTheme(Uri root, string name = null)
        {
            if (root == null) throw new ArgumentNullException("root");
            if (!root.IsFile) throw new ArgumentException("Root must be a path name.");

            _Root = root.AbsolutePath;
            _CurrentRoot = Path.Combine(_Root, "current");
            _UnpublishedRoot = Path.Combine(_Root, "unpublished");
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
            return _GetItem(_Items, name, type);
        }

        /// <summary>
        /// Returns a single unpublished Item if found.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IUnpublishedThemeItem GetUnpublishedItem(string name, string type)
        {
            return _GetItem(_UnpublishedItems, name, type);
        }

        /// <summary>
        /// Creates a new, unpublished item.
        /// A new item can only be unpublished.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public IUnpublishedThemeItem CreateUnpublishedItem(string name, string type, string content = "")
        {
            if (GetUnpublishedItem(name, type) == null)
            {
                // Create file...
                var path = Path.Combine(_UnpublishedRoot, type, name);
                File.WriteAllText(path, content);

                // ..re-enumerate file-system.
                _UnpublishedItems[type] = _EnumerateItems(_UnpublishedRoot, type, (f) => new UnpublishedItem(
                    name: Path.GetFileName(f),
                    type: type,
                    absolutePath: f));

                return _UnpublishedItems[type][name];
            }
            else throw new ArgumentException(string.Format("File {0}/{1} already exists!", type, name));
        }


        /// <summary>
        /// Renames an item inside its type
        /// A new item can only be unpublished.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public IUnpublishedThemeItem RenameUnpublishedItem(string name, string type, string newName)
        {
            if (GetUnpublishedItem(name, type) != null)
            {
                // Create file...
                var path = Path.Combine(_UnpublishedRoot, type, name);
                var newPath = Path.Combine(_UnpublishedRoot, type, newName);
                File.Move(path, newPath);

                // ..re-enumerate file-system.
                _UnpublishedItems[type] = _EnumerateItems(_UnpublishedRoot, type, (f) => new UnpublishedItem(
                    name: Path.GetFileName(f),
                    type: type,
                    absolutePath: f));

                return _UnpublishedItems[type][newName];
            }
            else throw new ArgumentException(string.Format("File {0}/{1} doesn't exists!", type, name));
        }


        /// <summary>
        /// Delete an unpublished item.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public void DeleteUnpublishedItem(string name, string type)
        {
            // Create file...
            var path = Path.Combine(_UnpublishedRoot, type, name);
            File.Delete(path);

            // ..re-enumerate file-system.
            _UnpublishedItems[type] = _EnumerateItems(_UnpublishedRoot, type, (f) => new UnpublishedItem(
                name: Path.GetFileName(f),
                type: type,
                absolutePath: f));
        }


        /// <summary>
        /// Replaces current version with the unpublished one.
        /// </summary>
        /// <returns></returns>
        public ITheme Publish()
        {
            lock (_PublishLock)
            {
                var currentTmpPath = Path.Combine(_Root, @"current_tmp");

                // Swap current and unpublished ...
                // ...and then delete the old current.
                Directory.Move(_CurrentRoot, currentTmpPath);
                Directory.Move(_UnpublishedRoot, _CurrentRoot);
                Directory.Delete(currentTmpPath, true);

                return new FileBasedTheme(new Uri(_Root));
            }
        }

        /// <summary>
        /// Generic method to get an item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private T _GetItem<T>(Dictionary<string, Dictionary<string, T>> items, string name, string type) where T : class
        {
            if (items != null && items.ContainsKey(type))
            {
                if (items[type].ContainsKey(name))
                {
                    return items[type][name];
                }
            }

            return null;
        }

        /// <summary>
        /// Get all the Items of a Theme type (i.e.: Assets, Templates, ...)
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected Dictionary<string, T> _EnumerateItems<T>(string path, string type, Func<string, T> createItemFn)
            where T : Item
        {
            var typePath = _isUnpublished(path)
                ? Path.Combine(path, type)
                : Path.Combine(path, "current", type);

            return Directory.EnumerateFiles(typePath)
                .Select(f => createItemFn(f))
                .ToDictionary(i => i.Name);
        }

        /// <summary>
        /// Loads alls the Theme-Information.
        /// </summary>
        protected void _Initialize(string name = null)
        {
            if (!Directory.Exists(_Root)) throw new ArgumentException(string.Format("Cannot find {0} of theme '{1}'.", Path.GetFileName(_Root), name));

            Name = name ?? Path.GetFileName(_Root);
            _Items = new Dictionary<string, Dictionary<string, Item>>();
            _UnpublishedItems = new Dictionary<string, Dictionary<string, UnpublishedItem>>();

            if (!Directory.Exists(_UnpublishedRoot))
            {
                // Create Unpublished if non existent.
                var currentPath = Path.Combine(_Root, @"current");
                var unpublishedPath = Path.Combine(_Root, @"unpublished");
                _copyDirectory(currentPath, _UnpublishedRoot, true);
            }

            foreach (var type in new string[] { "layouts", "assets", "templates", "snippets" })
            {
                _Items.Add(type, _EnumerateItems(_Root, type, (f) => new Item(
                    name: Path.GetFileName(f),
                    type: type,
                    absolutePath: f)));

                _UnpublishedItems.Add(type, _EnumerateItems(_UnpublishedRoot, type, (f) => new UnpublishedItem(
                    name: Path.GetFileName(f),
                    type: type,
                    absolutePath: f)));
            }
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
