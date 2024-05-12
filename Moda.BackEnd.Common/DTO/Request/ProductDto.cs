using Microsoft.AspNetCore.Http;
using Moda.Backend.Domain.Models;
using Moda.BackEnd.Domain.Enum;
using Moda.BackEnd.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Common.DTO.Request
{
    public class ProductDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public ClothType ClothType { get; set; }
        public Gender Gender { get; set; }
        public Guid ShopId { get; set; }
        public IEnumerable<Stock> ProductStocks { get; set; }   
        public File? File { get; set; }       
    }

    public class Stock
    {
        public Size.ClothingSize? ClothingSize { get; set; }
        public Size.ShoeSize? ShoeSize { get; set; }
        public int Quantity { get; set; }
        public Guid WarehouseId { get; set; }   
    }
    public class File
    {
        public IFormFile Img { get; set; } = null!;
    }
}
