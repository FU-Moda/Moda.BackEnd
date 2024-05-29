﻿using AutoMapper;
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
        private readonly IRepository<OptionPackageHistory> _optionPackageHistory;
        private readonly IUnitOfWork _unitOfWork;
        public OptionShopPackageService(
            IServiceProvider serviceProvider,
            IRepository<OptionPackage> repository,
            IRepository<OptionPackageHistory> optionPackageHistory,
            IUnitOfWork unitOfWork) : base(serviceProvider)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _optionPackageHistory = optionPackageHistory;   
        }

        public async Task<AppActionResult> CreateOptionPackage(OptionPackageHistoryDto optionPackageHistoryDto)
        {
            var result = new AppActionResult();
            try
            {
                var existingPackage = await _repository.GetByExpression(p => p!.PackageName == optionPackageHistoryDto.OptionPackageDto.PackageName);
                if (existingPackage != null)
                {
                    return BuildAppActionResultError(result, "A package with the same name already exists.");
                }
                var optionPackageDb = new OptionPackage
                {
                    OptionPackageId = Guid.NewGuid(),
                    PackageName = optionPackageHistoryDto.OptionPackageDto.PackageName,
                    Description = optionPackageHistoryDto.OptionPackageDto.Description,
                    Duration = optionPackageHistoryDto.OptionPackageDto.Duration,
                    Status = OptionPackageStatus.ACTIVE,
                };
                var optionPackageHistoryDb = new OptionPackageHistory
                {
                    OptionPackageHistoryId = Guid.NewGuid(),
                    OptionPackageId = optionPackageDb.OptionPackageId,
                    PackagePrice = optionPackageHistoryDto.PackagePrice,
                    Date = optionPackageHistoryDto.Date,
                };
            

                await _optionPackageHistory!.Insert(optionPackageHistoryDb);
                await _repository.Insert(optionPackageDb);
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
            var result = new AppActionResult();
            try
            {
                result.Result = await _optionPackageHistory.GetAllDataByExpression(null, pageNumber, pageSize, null, false, p => p.OptionPackage!);
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> GetOptionPackageById(Guid optionPackageId)
        {
            var result = new AppActionResult();
            try
            {
                result.Result = await _optionPackageHistory!.GetByExpression(p => p.OptionPackageId == optionPackageId, p => p.OptionPackage!);
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> UpdateOptionPackage(Guid packageId, OptionPackageHistoryDto optionPackageHistory)
        {
            var result = new AppActionResult();
            try
            {
                var optionPackageDb = await _repository.GetByExpression(p => p!.OptionPackageId == packageId);
                if (optionPackageDb == null)
                {
                    return BuildAppActionResultError(result, "Package này không tồn tại.");
                }

                optionPackageDb.PackageName = optionPackageHistory.OptionPackageDto.PackageName;
                optionPackageDb.Description = optionPackageHistory.OptionPackageDto.Description;
                optionPackageDb.Duration = optionPackageHistory.OptionPackageDto.Duration;
                var latestHistory = await _optionPackageHistory!.GetByExpression(h => h!.OptionPackageId == packageId);
                if (latestHistory != null)
                {
                    // Update the existing history record
                    latestHistory.PackagePrice = optionPackageHistory.PackagePrice;
                }
                else
                {
                    // Create a new OptionPackageHistory if no matching history found
                    var newOptionPackageHistoryDb = new OptionPackageHistory
                    {
                        OptionPackageHistoryId = Guid.NewGuid(),
                        OptionPackageId = optionPackageDb.OptionPackageId,
                        PackagePrice = optionPackageHistory.PackagePrice,
                        Date = optionPackageHistory.Date,
                    };
                    await _optionPackageHistory.Insert(newOptionPackageHistoryDb);
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
