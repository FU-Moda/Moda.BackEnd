using Moda.Backend.Domain.Models;
using Moda.BackEnd.Common.DTO.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Common.DTO.Response
{
    public class ShopDto
    {
        public Shop Shop { get; set; }
        public List<ProductResponseDto>? productResponseDtos { get; set; } = new List<ProductResponseDto>();
    }

    public class ProductResponseDto
    {
        public Product Product { get; set; }
        public string Img { get; set; }
    }
}
