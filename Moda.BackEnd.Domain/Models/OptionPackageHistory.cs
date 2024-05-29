using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Domain.Models
{
    public class OptionPackageHistory
    {
        [Key]
        public Guid OptionPackageHistoryId { get; set; }
        public double PackagePrice { get; set; }   
        public DateTime Date { get; set; }  
        public Guid OptionPackageId { get; set; }
        [ForeignKey(nameof(OptionPackageId))]
        public OptionPackage OptionPackage { get; set; } = null!; 
    }
}
