using Moda.Backend.Domain.Models;
using Moda.BackEnd.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Common.DTO.Request
{
    public class ProductReportResponse
    {
        public List<ProductReport> productReports { get; set; } = new List<ProductReport> { };
        public List<Tag> tags { get; set; } = new List<Tag> { };
    }

    public class ProductReport
    {
        public Product Product { get; set; }
        public double total { get; set; }
    }
}
