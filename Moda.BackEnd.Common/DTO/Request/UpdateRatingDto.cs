using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Common.DTO.Request
{
    public class UpdateRatingDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public int RatingPoint { get; set; }
        public string Content { get; set; } = string.Empty;
        public Guid ProductId { get; set; }
        public string AccountId { get; set; } = null!;
        public List<IFormFile>? Img { get; set; } = null!;
    }
}
