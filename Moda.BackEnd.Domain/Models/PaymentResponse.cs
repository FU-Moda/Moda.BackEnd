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
    public class PaymentResponse
    {
        [Key]
        public Guid PaymentResponseId { get; set; }
        public Guid OrderId { get; set; }
        [ForeignKey("OrderId")] 
        public Order Order { get; set; } = null!;
        public string? Amount { get; set; }
        public string? OrderInfo { get; set; }
        public bool Success { get; set; }
    }
}
