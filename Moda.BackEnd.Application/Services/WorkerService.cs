using AutoMapper;
using Hangfire;
using Microsoft.IdentityModel.Tokens;
using Moda.BackEnd.Application.IServices;
using Moda.BackEnd.Common.Utils;

namespace Moda.BackEnd.Application.Services
{
    public class WorkerService : GenericBackendService
    {
        private IShopPackageService _shopPackageService;
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public WorkerService(IUnitOfWork unitOfWork, IShopPackageService shopPackageService, IMapper mapper, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _shopPackageService = shopPackageService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Start()
        {
            TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            RecurringJob.AddOrUpdate(() => _shopPackageService.SendEndSupscriptionEmail(), Cron.DayInterval(1), vietnamTimeZone);
            RecurringJob.AddOrUpdate(() => _shopPackageService.UpdateShopPackageStatus(), Common.Utils.Utility.ConvertToCronExpression(8, 30), vietnamTimeZone);
        }

        public async Task CheckSendEmail()
        {
            try
            {
                var emailService = Resolve<IEmailService>();
                emailService.SendEmail("phamkhang12378@gmail.com",
                               "test ne",
                               "Hello");
            }
            catch (Exception ex)
            {
            }
            Task.CompletedTask.Wait();
        }
    }
}