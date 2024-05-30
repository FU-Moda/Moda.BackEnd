using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moda.BackEnd.API.Middlewares;
using Moda.BackEnd.Application.IServices;
using Moda.BackEnd.Common.DTO.Request;
using Moda.BackEnd.Common.DTO.Response;
using Moda.BackEnd.Domain.Enum;

namespace Moda.BackEnd.API.Controllers
{
    [Route("shop")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private IShopService _service;
        public ShopController(IShopService service)
        {
            _service = service;
        }

        [HttpGet("get-all-shop/{pageNumber}/{pageSize}")]
        [CacheAttribute(259200)]
        public async Task<AppActionResult> GetAllShop(int pageNumber = 1, int pageSize = 10)
        {
            return await _service.GetAllShop(pageNumber, pageSize);
        }

        [HttpGet("get-shop-by-id/{Id}")]
        [CacheAttribute(259200)]
        public async Task<AppActionResult> GetShopById(Guid Id)
        {
            return await _service.GetShopById(Id);
        }

        [HttpPut("update-shop")]
        [RemoveCacheAtrribute("shop")]
        public async Task<AppActionResult> UpdateShop([FromBody] UpdateShopDto dto)
        {
            return await _service.UpdateShop(dto);
        }

        [HttpPost("create-shop")]
        [RemoveCacheAtrribute("shop")]
        public async Task<AppActionResult> AddShop([FromBody] CreateShopDto dto)
        {
            return await _service.AddShop(dto);
        }

        [HttpGet("get-shop-by-accountId/{Id}")]
        [CacheAttribute(259200)]
        public async Task<AppActionResult> GetShopByAccountId(string Id)
        {
            return await _service.GetShopByAccountId(Id);
        }

        [HttpGet("get-shop-affiliate-by-shop-id/{shopId}/{pageNumber}/{pageSize}")]
        [CacheAttribute(259200)]
        public async Task<AppActionResult> GetShopAffiliateByShopId(Guid shopId, int pageNumber = 1, int pageSize = 10)
        {
            return await _service.GetShopAffiliateByShopId(shopId, pageNumber, pageSize);
        }

        [HttpGet("get-shop-with-banner/{pageNumber}/{pageSize}")]
        public async Task<AppActionResult> GetShopWithBanner(int pageNumber = 1, int pageSize = 10)
        {
            return await _service.GetShopWithBanner(pageNumber, pageSize);
        }

        [HttpGet("get-shop-package-by-status/{pageNumber}/{pageSize}")]
        public async Task<AppActionResult> GetShopPackageByStatus(ShopPackageStatus shopPackageStatus, int pageNumber = 1, int pageSize = 10)
        {
            return await _service.GetShopPackageByStatus(shopPackageStatus, pageNumber, pageSize);
        }

        [HttpPost("assign-package-for-shop")]
        [RemoveCacheAtrribute("shop")]
        public async Task<AppActionResult> AssignPackageForShop(Guid shopId, Guid optionPackageId)
        {
            return await _service.AssignPackageForShop(shopId, optionPackageId, HttpContext);
        }

        [HttpPut("update-package-status-for-shop/{shopId}")]
        [RemoveCacheAtrribute("shop")]
        public async Task<AppActionResult> UpdatePackageStatusForShop(Guid shopId, ShopPackageStatus shopPackageStatus)
        {
            return await _service.UpdatePackageStatusForShop(shopId, shopPackageStatus);   
        }
        [HttpGet("get-total-affiliate")]
        [CacheAttribute(259200)]
        public async Task<AppActionResult> GetTotalAffiliate(Guid? shopId, DateTime startDate, DateTime endDate)
        {
            return await _service.GetTotalAffiliate(shopId, startDate, endDate);
        }

        [HttpGet("get-total-order-detail-affiliate")]
        public async Task<AppActionResult> GetTotalOrderDetailAffiliate(Guid? shopId, DateTime startDate, DateTime endDate)
        {
            return await _service.GetTotalOrderDetailAffiliate(shopId, startDate, endDate);
        }

        [RemoveCacheAtrribute("shop")]
        [HttpGet("remove-cache")]
        public IActionResult RemoveCache()
        {
            return Ok();
        }
    }
}
