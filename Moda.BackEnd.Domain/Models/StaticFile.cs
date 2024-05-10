using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.Backend.Domain.Models
{
    public class StaticFile
    {
        [Key]
        public Guid Id { get; set; }
        public string Img { get; set; } = null!;
        public Guid? ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }
        public Guid? RatingId { get; set; }
        [ForeignKey(nameof(RatingId))]
        public Rating? Rating { get; set; }
}
}
