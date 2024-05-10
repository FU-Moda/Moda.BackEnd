using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Domain.Models
{
    public class Coupon
    {
        [Key]
        public Guid Id { get; set; }
        public string Code { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double ConditionAmount { get; set; }
        public double? Percent { get; set; }
        public double? Amount { get; set; }
    }
}
