using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Llprk.DataAccess.Models.Theme
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

        string Name { get; }
        IThemeItem[] Assets { get; }
        IThemeItem[] Layouts { get; }
        IThemeItem[] Snippets { get; }
        IThemeItem[] Templates { get; }

        Dictionary<string, Dictionary<string, IUnpublishedThemeItem>> UnpublishedItems { get; }

        ITheme Publish();

        IThemeItem GetItem(string name, string type);
        IUnpublishedThemeItem GetUnpublishedItem(string name, string type);
        IUnpublishedThemeItem CreateUnpublishedItem(string name, string type, string content = null);
    }

    public interface IThemeItem
    {
        string Name { get; }
        string AbsolutePath { get; }
        string Type { get; }
        string ContentType { get; }

        string ReadContent();
    }

    public interface IUnpublishedThemeItem : IThemeItem
    {
        void WriteContent(string content);
        void Rename(string newName);
    }

}
