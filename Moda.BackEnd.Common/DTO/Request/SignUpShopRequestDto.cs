using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Common.DTO.Request
{
    public class SignUpShopRequestDto
    {
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool Gender { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string ShopName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Address { get; set; } = null!;
    }
}
