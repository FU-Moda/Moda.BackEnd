using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Common.DTO.Request
{
    public class CartDetailDto
    {
        public Guid ProductId { get; set; } 
        public int Count { get; set; }      
    }
}
