﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Common.DTO.Response
{
    public class AccountResponse
    {
        public string Id { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public bool Gender { get; set; }
        public bool IsVerified { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public object Role { get; set; }
    }
}
