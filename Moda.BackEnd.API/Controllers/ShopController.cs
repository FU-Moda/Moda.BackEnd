using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moda.BackEnd.API.Middlewares;
using Moda.BackEnd.Application.IServices;
using Moda.BackEnd.Common.DTO.Request;
using Moda.BackEnd.Common.DTO.Response;

namespace Moda.BackEnd.API.Controllers
{
    [Route("api/shop")]
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

        [HttpPost("update-shop")]
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
    }
}
