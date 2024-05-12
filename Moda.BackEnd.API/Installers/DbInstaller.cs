
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moda.BackEnd.Domain.Data;
using Moda.BackEnd.Domain.Models;

namespace Moda.BackEnd.API.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ModaDbContext>(option =>
            {
                option.UseSqlServer(configuration["ConnectionStrings:DB"]);
            });

            services.AddIdentity<Account, IdentityRole>().AddEntityFrameworkStores<ModaDbContext>()
     .AddDefaultTokenProviders();
        }
    }
}
