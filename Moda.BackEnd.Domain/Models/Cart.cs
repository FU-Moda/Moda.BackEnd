using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Domain.Models
{
    public class Cart
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ProductStockId { get; set; }
        [ForeignKey(nameof(ProductStockId))]
        public ProductStock? ProductStock { get; set; }
        public int Size { get; set; }   
        public double Total { get; set; }   
    }
}
