using Microsoft.AspNetCore.Http;
using Moda.BackEnd.Common.DTO.Request;
using Moda.BackEnd.Common.DTO.Response;
using Moda.BackEnd.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Application.IServices
{
    public interface IShopService
    {
        public Task<AppActionResult> GetAllShop(int pageNumber, int pageSize);
        public Task<AppActionResult> GetShopById(Guid Id);
        public Task<AppActionResult> AddShop(CreateShopDto dto);
        public Task<AppActionResult> UpdateShop(UpdateShopDto dto);
        public Task<AppActionResult> GetShopByAccountId(string Id);
        public Task<AppActionResult> GetShopAffiliateByShopId(Guid shopId, int pageNumber, int pageSize);
        public Task<AppActionResult> GetTotalAffiliate(Guid? shopId, DateTime startDate, DateTime endDate);
        public Task<AppActionResult> GetTotalOrderDetailAffiliate(Guid? shopId, DateTime startDate, DateTime endDate);
        public Task<AppActionResult> GetShopWithBanner(int pageNumber, int pageSize);
        public Task<AppActionResult> GetShopPackageByStatus(ShopPackageStatus shopPackageStatus, int pageNumber, int pageSize);
        public Task<AppActionResult> AssignPackageForShop(Guid shopId, Guid optionPackageId, HttpContext context);
        public Task<AppActionResult> UpdatePackageStatusForShop(Guid shopId, ShopPackageStatus shopPackageStatus);
        public Task<AppActionResult> CheckShopPackageSubscription(Guid shopId);
    }
}
