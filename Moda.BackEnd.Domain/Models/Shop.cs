using Moda.BackEnd.Domain.Models;
using Moda.BackEnd.Domain.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.Backend.Domain.Models
{
    public class Shop
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string AccountId { get; set; } = null!;
        [ForeignKey(nameof(AccountId))]
        public Account Account { get; set; } = null!;
    }
}
