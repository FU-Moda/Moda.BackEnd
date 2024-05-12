using Moda.BackEnd.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.Backend.Domain.Models
{
    public class Rating
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public int RatingPoint { get; set; }
        public string Content { get; set; } = string.Empty;
        public string? AccountId { get; set; }
        [ForeignKey(nameof(AccountId))]
        public Account? Account { get; set; }
        public Guid ProductId { get; set; }  
        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }
    }
}
