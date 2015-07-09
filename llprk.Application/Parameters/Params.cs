using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Llprk.Application.Parameters
{
    public class Params
    {
        public int? Page { get; set; }
        public int? Count { get; set; }
        public string OrderBy { get; set; }
        public bool Desc { get; set; }

        public Params()
        {
        }

        public Params(Params ps) : base()
        {
            if (ps != null) {
                Page = ps.Page;
                Count = ps.Count;
                OrderBy = ps.OrderBy;
                Desc = ps.Desc;
            }
        }
    }

}