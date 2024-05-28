using Moda.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Domain.Models
{
    public class ShopPackage
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ShopId { get; set; }
        [ForeignKey(nameof(ShopId))]
        public Shop Shop { get; set; } = null!;
        public Guid OptionPackageHistoryId;
        [ForeignKey(nameof(OptionPackageHistoryId))]
        public OptionPackageHistory OptionPackageHistory { get; set; } = null!;
    }
}
