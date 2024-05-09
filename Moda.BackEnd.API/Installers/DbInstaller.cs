
using Microsoft.EntityFrameworkCore;
using Moda.BackEnd.Domain.Data;

namespace Moda.BackEnd.API.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ModaDbContext>(option =>
            {
                option.UseSqlServer(configuration["ConnectionStrings:Host"]);
            });
        }
    }
}
