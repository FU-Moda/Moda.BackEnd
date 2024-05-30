using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Common.DTO.Request
{
    public class ConfigurationDto
    {
        public string Name { get; set; } = null!;
        public string PreValue { get; set; } = null!;
        public string ActiveValue { get; set; } = null!;
        public DateTime ActiveDate { get; set; }
    }
}
