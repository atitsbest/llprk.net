﻿using Llprk.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Llprk.Web.UI.Areas.Admin.Models
{
    public class ProductDelete
    {
        public Product Product { get; set; }
        public IEnumerable<Order> Orders { get; set; }
    }
}