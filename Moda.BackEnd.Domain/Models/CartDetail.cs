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
    public class CartDetail
    {
        [Key]
        public Guid Id { get; set; }
        public Guid CartId { get; set; } 
        [ForeignKey(nameof(CartId))]
        public Cart? Cart { get; set; }     
        public Guid ProductId { get; set; } 
        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; } 
        public int Count { get; set; }
    }
}
