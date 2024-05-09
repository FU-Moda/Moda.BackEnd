using Moda.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Domain.Models
{
    public class ProductTag
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }
        public Guid TagId { get; set; }
        [ForeignKey(nameof(TagId))]
        public Product? Tag { get; set; }
    }
}
