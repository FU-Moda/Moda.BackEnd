using Microsoft.AspNetCore.Identity;
using Moda.BackEnd.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Common.DTO.Response
{
    public class AccountRoleResponse
    {
        public Account Account { get; set; } = null!;
        public List<IdentityRole> IdentityRoles { get; set; } = null!;     
    }
}
