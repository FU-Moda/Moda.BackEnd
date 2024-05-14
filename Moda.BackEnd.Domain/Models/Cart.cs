using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Domain.Models
{
    public class Cart
    {
        [Key]
        public Guid Id { get; set; }
        public string AccountId { get; set; } = null!;
        [ForeignKey(nameof(AccountId))]
        public Account? Account { get; set; }
        public double Total { get; set; }   
    }
}
