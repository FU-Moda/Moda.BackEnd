using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Common.DTO.Request
{
    public class OptionPackageDto
    {
        public string PackageName { get; set; } = null!;
        public DateTime Duration { get; set; }
        public string Description { get; set; } = null!;
        public double Price { get; set; }   
        public DateTime Date { get; set; }
    }
  
}
