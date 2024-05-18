using Moda.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Domain.Models
{
    public class GroupedOrderDetails
    {
        public Guid OrderId { get; set; }
        public List<OrderDetail> OrderDetails { get; set; } = null!;
    }
}
