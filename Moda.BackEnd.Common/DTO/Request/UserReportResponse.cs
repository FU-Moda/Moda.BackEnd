using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Common.DTO.Request
{
    public class UserReportResponse
    {
        public int totalCustomer {  get; set; }
        public int newCustomer { get; set; }
        public int? totalAdmin { get; set; }
        public int? totalStaff { get; set; }
    }
}
