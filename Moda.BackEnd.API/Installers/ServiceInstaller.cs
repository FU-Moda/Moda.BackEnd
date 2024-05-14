
using Moda.BackEnd.Application;
using Moda.BackEnd.Application.IRepositories;
using Moda.BackEnd.Application.IServices;
using Moda.BackEnd.Application.Services;
using Moda.BackEnd.Domain.Data;
using Moda.BackEnd.Infrastructure.Repositories;
using Moda.BackEnd.Infrastructure.UnitOfWork;

namespace Moda.BackEnd.API.Installers
{
    public class ServiceInstaller : IInstaller
    {
        public void InstallService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient();
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IDbContext, ModaDbContext>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDbContext, ModaDbContext>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IFirebaseService, FirebaseService>();
            services.AddScoped<IShopService, ShopService>();
            services.AddScoped<IRatingService, RatingService>();
        }
    }
}
