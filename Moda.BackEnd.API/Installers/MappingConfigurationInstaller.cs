
using Moda.BackEnd.Common.ConfigurationModel;

namespace Moda.BackEnd.API.Installers
{
    public class MappingConfigurationInstaller : IInstaller
    {
        public void InstallService(IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfiguration = new JWTConfiguration();
            configuration.GetSection("JWT").Bind(jwtConfiguration);
            services.AddSingleton(jwtConfiguration);


            var firebaseAdminSdkConfiguration = new FirebaseAdminSDK();
            configuration.GetSection("FirebaseAdminSDK").Bind(firebaseAdminSdkConfiguration);
            services.AddSingleton(firebaseAdminSdkConfiguration);

            var emailConfiguration = new EmailConfiguration();
            configuration.GetSection("Email").Bind(emailConfiguration);
            services.AddSingleton(emailConfiguration);

            var firebaseConfiguration = new FirebaseConfiguration();
            configuration.GetSection("Firebase").Bind(firebaseConfiguration);
            services.AddSingleton(firebaseConfiguration);


        }
    }
}
