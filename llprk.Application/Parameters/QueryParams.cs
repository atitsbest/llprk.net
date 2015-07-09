using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Llprk.Application.Parameters
{
    public class QueryParams : Params
    {
        public class QueryDef
        {
            public string Query { get; set; }
        }

        public QueryDef Query { get; set; }

        /// <summary>
        /// CTR
        /// </summary>
        public QueryParams()
        {
            Query = new QueryDef();
        }
    }

}