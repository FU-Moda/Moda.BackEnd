using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Common.ConfigurationModel
{
    public class EmailConfiguration
    {
        public string User { get; set; } = null!;
        public string ApplicationPassword { get; set; } = null!;
    }
}
