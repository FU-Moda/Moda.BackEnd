using Moda.BackEnd.Domain.Enum;
using Moda.BackEnd.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.Backend.Domain.Models
{
    public class StockHistory
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime Date { get; set; }    
        public ActionType ActionType { get; set; }
        public int Quantity { get; set; }
        public Guid ProductStockId { get; set; }
        [ForeignKey(nameof(ProductStockId))]
        public ProductStock ProductStock { get; set; }
    }
}
