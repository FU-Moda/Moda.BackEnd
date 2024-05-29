using Moda.BackEnd.Domain.Enum;
using Moda.BackEnd.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Common.DTO.Request
{
    public class OptionPackageHistoryDto
    {
        public double PackagePrice { get; set; }
        public OptionPackageDto OptionPackageDto { get; set; } = null!;
    }
    public class OptionPackageDto
    {
        public string PackageName { get; set; } = null!;
        public DateTime Duration { get; set; }
        public string Description { get; set; } = null!;
        public OptionPackageStatus Status { get; set; }
        public OptionPackagePriority Priority { get; set; }
        public bool IsDashboardAvailable { get; set; }
        public bool IsBannerAvailable { get; set; }
    }
}
