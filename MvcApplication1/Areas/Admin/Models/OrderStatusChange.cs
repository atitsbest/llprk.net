﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Llprk.DataAccess.Models;
using System.ComponentModel.DataAnnotations;

namespace Llprk.Web.UI.Areas.Admin.Models
{
    /// <summary>
    /// ViewModel für ~/orders/new
    /// </summary>
    public class OrderStatusChange
    {
        public Order Order { get; set; }
        public string ChangeActionName { get; set; }
        public string Headline { get; set; }
        public string ActionButtonText { get; set; }
        public string MailBody { get; set; }
    }
}