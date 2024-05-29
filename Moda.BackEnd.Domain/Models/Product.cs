using Moda.BackEnd.Domain.Enum;
using Moda.BackEnd.Domain.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.Backend.Domain.Models
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public ClothType ClothType { get; set; }
        public Gender Gender { get; set; }
        public Guid ShopId { get; set; }
        [ForeignKey(nameof(ShopId))]
        public Shop? Shop { get; set; }
        public ProductStatus Status { get; set; }       
    }
}
