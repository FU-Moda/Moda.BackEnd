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
    public class Order
    {
        [Key]
        public Guid Id { get; set; }
        public string Address { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public OrderStatus Status { get; set; }
        public double Total { get; set; }
        public double DeliveryCost { get; set; }
        public string AccountId { get; set; } = null!;
        [ForeignKey(nameof(AccountId))]
        public Account? Account { get; set; }    
        public DateTime OrderTime { get; set; }
        public Guid CouponId { get; set; }
        [ForeignKey(nameof(CouponId))]
        public Coupon? Coupon { get; set;}
    }
}
