using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Moda.BackEnd.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Domain.Data
{
    public class ModaDbContext : IdentityDbContext<Account>, IDbContext
    {

        public ModaDbContext()
        {
        }
        public ModaDbContext(DbContextOptions options) : base(options)
        {
        }


    }
}
