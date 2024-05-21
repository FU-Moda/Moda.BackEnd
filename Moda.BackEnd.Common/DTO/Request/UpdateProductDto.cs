using Microsoft.AspNetCore.Http;
using Moda.BackEnd.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Common.DTO.Request
{
    
    public class UpdateProductDto
    {
        public Guid Id { get; set; }        
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public ClothType ClothType { get; set; }
        public Gender Gender { get; set; }
        public Guid ShopId { get; set; }
        public IEnumerable<UpdateStock>? ProductStocks { get; set; }
        public List<IFormFile> Img { get; set; } = null!;

    }

    public class UpdateStock
    {
        public Size.ClothingSize? ClothingSize { get; set; }
        public Size.ShoeSize? ShoeSize { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public Guid WarehouseId { get; set; }
    }
}
