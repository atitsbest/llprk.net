using Llprk.Application.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Llprk.Application.DTOs.Requests
{
    public class UpdateLineItemQtyRequest
    {
        public int Id { get; set; }
        public int Qty { get; set; }
    }
}
