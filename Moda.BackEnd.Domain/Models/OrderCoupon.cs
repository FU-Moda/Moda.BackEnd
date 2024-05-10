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
    public class OrderCoupon
    {
        [Key]
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }
        [ForeignKey(nameof(OrderId))]
        public Order? Order { get; set; }
        public Guid CouponId { get; set; }
        [ForeignKey(nameof(CouponId))]
        public Coupon? Coupon { get; set; }
    }
}
