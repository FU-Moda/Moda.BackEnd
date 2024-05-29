using Moda.BackEnd.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Common.DTO.Response
{
    public class OptionPackageResponse
    {
        public OptionPackage OptionPackage { get; set; } = null!;
        public List<OptionPackageHistory> OptionPackageHistory { get; set; } = null!;
    }

    public class OptionPackageResponses
    {
        public OptionPackage OptionPackage { get; set; } = null!;
        public OptionPackageHistory OptionPackageHistory { get; set; } = null!;
    }
}
