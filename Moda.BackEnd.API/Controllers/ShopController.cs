using Microsoft.AspNetCore.Mvc;
using Moda.BackEnd.Application.IServices;
using Moda.BackEnd.Common.DTO.Request;
using Moda.BackEnd.Common.DTO.Response;

namespace Moda.BackEnd.API.Controllers
{
    public class ShopController : Controller
    {
        private IShopService _service;
        public ShopController(IShopService service)
        {
            _service = service;
        }

        [HttpGet("get-all-shop/{pageNumber}/{pageSize}")]
        public async Task<AppActionResult> GetAllShop(int pageNumber = 1, int pageSize = 10)
        {
            return await _service.GetAllShop(pageNumber, pageSize);
        }

        [HttpGet("get-shop-by-id/{Id}")]
        public async Task<AppActionResult> GetShopById(Guid Id)
        {
            return await _service.GetShopById(Id);
        }

        [HttpPost("update-shop")]
        public async Task<AppActionResult> UpdateShop([FromBody]UpdateShopDto dto)
        {
            return await _service.UpdateShop(dto);
        }

        [HttpPost("create-shop")]
        public async Task<AppActionResult> AddShop([FromBody]CreateShopDto dto)
        {
            return await _service.AddShop(dto);
        }
    }
}
