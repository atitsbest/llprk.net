using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Llprk.Web.UI.Application.Exceptions
{
    public class AppException : Exception
    {
        public AppException(string msg) : base(msg) { }
    }
}