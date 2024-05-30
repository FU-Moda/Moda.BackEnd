using Moda.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Common.DTO.Response
{
    public class OrderProfitListResponse
    {
        public double TotalProfit { get; set; }
        public List<OrderProfitResponse> orderProfitResponses { get; set; } = new List<OrderProfitResponse>();
    }

    public class OrderProfitResponse
    {
        public double Profit { get; set; }
        public Order Order { get; set; }
    }
}
