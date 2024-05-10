using Moda.BackEnd.Domain.Enum;
using Moda.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Domain.Models
{
    public class ProductStock
    {
        [Key]
        public Guid Id { get; set; }
        public Size.ClothingSize? ClothingSize { get; set; }
        public Size.ShoeSize? ShoeSize { get; set; }
        public int Quantity { get; set; }
        public Guid ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }
        public Guid WarehouseId { get; set; }
        [ForeignKey(nameof(WarehouseId))]
        public Warehouse? Warehouse { get; set; }
    }
}
