using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Application.Exceptions
{
    public class AppException : Exception
    {
        public AppException(string msg) : base(msg) { }
    }
}