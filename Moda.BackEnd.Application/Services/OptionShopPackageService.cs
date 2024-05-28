using AutoMapper;
using Moda.BackEnd.Application.IRepositories;
using Moda.BackEnd.Application.IServices;
using Moda.BackEnd.Common.DTO.Request;
using Moda.BackEnd.Common.DTO.Response;
using Moda.BackEnd.Domain.Enum;
using Moda.BackEnd.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Application.Services
{
    public class OptionShopPackageService : GenericBackendService, IOptionPackageService
    {
        private readonly IRepository<OptionPackage> _repository;
        private readonly IUnitOfWork _unitOfWork;
        public OptionShopPackageService(
            IServiceProvider serviceProvider,
            IRepository<OptionPackage> repository,
            IUnitOfWork unitOfWork) : base(serviceProvider)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AppActionResult> CreateOptionPackage(OptionPackageDto optionPackageDto)
        {
            var result = new AppActionResult();
            var optionPackageHistory = Resolve<IRepository<OptionPackageHistory>>();
            try
            {
                var existingPackage = await _repository.GetByExpression(p => p!.PackageName == optionPackageDto.PackageName);
                if (existingPackage != null)
                {
                    return BuildAppActionResultError(result, "A package with the same name already exists.");
                }
                var optionPackageDb = new OptionPackage
                {
                    OptionPackageId = Guid.NewGuid(),
                    PackageName = optionPackageDto.PackageName,
                    Description = optionPackageDto.Description,
                    Duration = optionPackageDto.Duration,
                    Status = OptionPackageStatus.ACTIVE,
                };

                var optionPackageHistoryDb = new OptionPackageHistory
                {
                    OptionPackageHistoryId = Guid.NewGuid(),
                    OptionPackageId = optionPackageDb.OptionPackageId,
                    PackagePrice = optionPackageDto.Price,
                    Date = optionPackageDto.Date,
                };

                await _repository.Insert(optionPackageDb);
                await optionPackageHistory!.Insert(optionPackageHistoryDb);
                await _unitOfWork.SaveChangesAsync();
                result.Messages.Add("Tạo Package thành công");
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> DeleteOptionPackage(Guid packageId)
        {
            var result = new AppActionResult();
            try
            {

                var packageDb = await _repository.GetByExpression(p => p.OptionPackageId == packageId);
                if (packageDb == null)
                {
                    result = BuildAppActionResultError(result, "Package này không tồn tại");
                }
                packageDb!.Status = OptionPackageStatus.INACTIVE;    
                await _unitOfWork.SaveChangesAsync();   
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> GetAllOptionPackage(int pageNumber, int pageSize)
        {
            var optionPackageHistory = Resolve<IRepository<OptionPackageHistory>>();
            var result = new AppActionResult();
            try
            {
                result.Result = await optionPackageHistory!.GetAllDataByExpression(null, pageNumber, pageSize, null, false, p => p.OptionPackage!);
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> GetOptionPackageById(Guid optionPackageId)
        {
            var optionPackageHistory = Resolve<IRepository<OptionPackageHistory>>();
            var result = new AppActionResult();
            try
            {
                result.Result = await optionPackageHistory!.GetByExpression(p => p.OptionPackageId == optionPackageId, p => p.OptionPackage!);
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> UpdateOptionPackage(Guid packageId, OptionPackageDto optionPackageDto)
        {
            var result = new AppActionResult();
            var optionPackageHistoryRepository = Resolve<IRepository<OptionPackageHistory>>();
            try
            {
                var optionPackageDb = await _repository.GetByExpression(p => p!.OptionPackageId == packageId);
                if (optionPackageDb == null)
                {
                    return BuildAppActionResultError(result, "Package này không tồn tại.");
                }

                optionPackageDb.PackageName = optionPackageDto.PackageName;
                optionPackageDb.Description = optionPackageDto.Description;
                optionPackageDb.Duration = optionPackageDto.Duration;
                var latestHistory = await optionPackageHistoryRepository!.GetByExpression(h => h!.OptionPackageId == packageId);
                if (latestHistory != null)
                {
                    // Update the existing history record
                    latestHistory.PackagePrice = optionPackageDto.Price;
                }
                else
                {
                    // Create a new OptionPackageHistory if no matching history found
                    var newOptionPackageHistoryDb = new OptionPackageHistory
                    {
                        OptionPackageHistoryId = Guid.NewGuid(),
                        OptionPackageId = optionPackageDb.OptionPackageId,
                        PackagePrice = optionPackageDto.Price,
                        Date = optionPackageDto.Date,
                    };
                    await optionPackageHistoryRepository.Insert(newOptionPackageHistoryDb);
                }

                // Update the package in the repository
                await _repository.Update(optionPackageDb);
                await _unitOfWork.SaveChangesAsync();

                result.Messages.Add("Cập nhật Package thành công.");
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }
    }
}
