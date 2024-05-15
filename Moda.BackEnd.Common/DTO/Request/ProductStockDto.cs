using Moda.Backend.Domain.Models;
using Moda.BackEnd.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Common.DTO.Request
{
    public class ProductStockDto
    {
        public Guid Id { get; set; }    
        public Size.ClothingSize? ClothingSize { get; set; }
        public Size.ShoeSize? ShoeSize { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public Guid ProductId { get; set; }
        public Guid WarehouseId { get; set; }
    }
}
