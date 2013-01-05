using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcApplication1.Models
{
    public class OrderLine
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public int ProductId { get; set; }

        public int Qty { get; set; }

        public virtual Product Product { get; set; }
        public virtual Order Order { get; set; }
    }
}