using Moda.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Common.DTO.Response
{
    public class RatingResponse
    {
        public Rating Rating { get; set; } = null!;
        public List<string> Image { get; set; } = null!; 

    }
}
