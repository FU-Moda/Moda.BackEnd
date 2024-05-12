using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Common.DTO.Request
{
    public class StaticFileDto
    {
        public Guid Id { get; set; }
        public IFormFile Img { get; set; } = null!;
        public Guid? ProductId { get; set; }
        public Guid? RatingId { get; set; }
    }
}
