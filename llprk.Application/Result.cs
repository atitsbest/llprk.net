using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Reflection;
using Llprk.Application.Parameters;

namespace Llprk.Application
{
    public class Result<T> : Params
    {
        public IEnumerable<T> Data { get; private set; }
        public int Total { get; set; }

        public Result(Params ps)
            : base(ps)
        { }

        public Result(Params ps, IQueryable<T> data)
            : base(ps)
        {
            SetData(data);
        }

        public void SetData(IQueryable<T> data)
        {
            var result = data;

            // Sortieren...
            if (!string.IsNullOrWhiteSpace(OrderBy))
            {
                result = result.OrderBy(string.Format("{0} {1}", OrderBy, Desc ? "desc" : ""));
            }
            else
            {
                // Vor einem Skip (siehe unten) muß ein OrderBy aufgerufen werden.
                // Ist keine Sortierung angegeben, müssen wir dennoch sortieren und behalten
                // dabei die Reihenfolge bei.
                result = result.OrderBy(x => true);
            }

            Total = result.Count();
            if (Count.HasValue)
            {
                Count = Math.Min(Count.Value, Total);
            }

            // Pagen...
            if (Page.HasValue) {
                if (!Count.HasValue) throw new ArgumentNullException("Bei angegebener Seite (page) muss auch die Anzahl der Einträge (count) angegeben werden!");
                // TODO: Kann diese Seite überhaupt angezeigt werden?
                result = result.Skip((Page.Value - 1) * Count.Value).Take(Count.Value);
            }

            Data = result;
        }
    }

}