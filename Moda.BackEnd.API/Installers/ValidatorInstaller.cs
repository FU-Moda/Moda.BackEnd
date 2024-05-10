
using Moda.BackEnd.Common.Validator;

namespace Moda.BackEnd.API.Installers
{
    public class ValidatorInstaller : IInstaller
    {
        public void InstallService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<HandleErrorValidator>();
        }
    }
}
