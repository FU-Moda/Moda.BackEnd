using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Common.DTO.Request
{
    public class OrderRequest
    {
        public string AccountId { get; set; } = null!;
        public List<ProductStockDto> ProductStockDtos { get; set; } = null!;
    }
}
