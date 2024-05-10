
using Moda.BackEnd.Application.IRepositories;
using Moda.BackEnd.Domain.Data;
using Moda.BackEnd.Infrastructure.Repositories;

namespace Moda.BackEnd.API.Installers
{
    public class ServiceInstaller : IInstaller
    {
        public void InstallService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient();
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IDbContext, ModaDbContext>();
        }
    }
}
