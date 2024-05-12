
using DinkToPdf;
using DinkToPdf.Contracts;
using Firebase.Storage;
using Moda.BackEnd.Common.Utils;
using Moda.BackEnd.Infrastructure.Mapping;
using System.ComponentModel;

namespace Moda.BackEnd.API.Installers
{
    public class LibraryInstaller : IInstaller
    {
        public void InstallService(IServiceCollection services, IConfiguration configuration)
        {
            var mapper = MappingConfig.RegisterMap().CreateMapper();
            services.AddSingleton(mapper);
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSignalR();
            services.AddSingleton<BackEndLogger>();
            services.AddSingleton(_ => new FirebaseStorage(configuration["Firebase:Bucket"]));
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            services.AddSingleton<Utility>();
            services.AddSingleton<SD>();
            services.AddSingleton<TemplateMappingHelper>();
        }
    }
}
