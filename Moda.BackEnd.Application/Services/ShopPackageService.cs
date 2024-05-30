using AutoMapper;
using Moda.BackEnd.Application.IRepositories;
using Moda.BackEnd.Application.IServices;
using Moda.BackEnd.Common.DTO.Response;
using Moda.BackEnd.Common.Utils;
using Moda.BackEnd.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Application.Services
{
    public class ShopPackageService : GenericBackendService, IShopPackageService
    {
        public readonly IRepository<ShopPackage> _repository;
        public readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ShopPackageService(
            IServiceProvider serviceProvider,
            IRepository<ShopPackage> repository,
            IEmailService emailService,
            IMapper mapper,
            IUnitOfWork unitOfWork
            ) : base(serviceProvider)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        public async Task<AppActionResult> SendEndSupscriptionEmail()
        {
            AppActionResult result = new AppActionResult(); 
            try
            {
                var utility = Resolve<Utility>();
                DateTime now = utility!.GetCurrentDateTimeInTimeZone();

                var shopPackageDb = await _repository.GetAllDataByExpression(s => s.RegisteredDate.HasValue &&
                                                 s.RegisteredDate.Value.Add(s.OptionPackageHistory.OptionPackage.Duration - DateTime.MinValue).AddDays(3) >= now,
                                            0, 0, null, false, s => s.Shop.Account);
                if(shopPackageDb.Items != null && shopPackageDb.Items.Count > 0)
                {
                    foreach (var shopPackage in shopPackageDb.Items!)
                    {

                        _emailService.SendEmail(shopPackage.Shop.Account.Email, $"Thông bao sắp hết hạn gói dịch vụ tại nền tảng MODA", "Gói dịch vụ của bạn chỉ còn 3 ngày nữa sẽ ht6 hạn. \n Nếu tiếp tục đồng hành cùng chúng mình, bạn đừng quên tiếp tục dag98 kí nhé");
                    }
                }
                await _unitOfWork.SaveChangesAsync();

            } catch ( Exception ex )
            {
                result = new AppActionResult();
            }
            return result;
        }

        public async Task<AppActionResult> UpdateShopPackageStatus()
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var utility = Resolve<Utility>();
                DateTime now = utility!.GetCurrentDateTimeInTimeZone();

                var shopPackageDb = await _repository.GetAllDataByExpression(s => s.RegisteredDate.HasValue &&
                                                 s.RegisteredDate.Value.Add(s.OptionPackageHistory.OptionPackage.Duration - DateTime.MinValue) >= now,
                                            0, 0, null, false, null);
                if (shopPackageDb.Items != null && shopPackageDb.Items.Count > 0)
                {
                    foreach (var shopPackage in shopPackageDb.Items!)
                    {
                        shopPackage.IsValid = false;
                        await _repository.Update(shopPackage);
                    }
                }
                await _unitOfWork.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                result = new AppActionResult();
            }
            return result;
        }
    }
}
