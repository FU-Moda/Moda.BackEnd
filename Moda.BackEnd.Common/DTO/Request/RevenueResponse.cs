using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Common.DTO.Request
{
    public class RevenueResponse
    {
        //// num of order, min max order value, revenue, order value/người, order count/ người
        public double Total { get; set; }
        public int NumOfOrder {  get; set; }
        public double MinOrderValue { get; set; }
        public double MaxOrderValue { get; set; }
        public double AvarageOrderValue { get; set; }
    }
}
