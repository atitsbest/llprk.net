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

        string Name { get; set; }
        IThemeItem[] Assets { get; }
        IThemeItem[] Layouts { get; }
        IThemeItem[] Snippets { get; }
        IThemeItem[] Templates { get; }

        IUnpublishedTheme Unpublished { get; }

        IThemeItem GetItem(string name, string type);
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
        string AbsolutePath { get; set; }
        string Type { get; set; }
        string ContentType { get; set; }

        string ReadContent();
    }


}
