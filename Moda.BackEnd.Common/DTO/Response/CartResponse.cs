using Moda.BackEnd.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Common.DTO.Response
{
    public class CartResponse
    {
        public Cart Cart { get; set; } = null!;
        public List<CartDetail> CartDetail { get; set; } = null!;   
    }
}
