using Hangfire;
using Moda.BackEnd.Application.Services;

namespace Moda.BackEnd.API.Installers
{
    public class HangfireInstaller : IInstaller
    {
        public void InstallService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(x => x.UseSqlServerStorage(configuration["ConnectionStrings:DB"]));
            services.AddHangfireServer();
            services.AddScoped<WorkerService>();
        }
    }
}