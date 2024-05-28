using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moda.BackEnd.API.Middlewares;
using Moda.BackEnd.Application.IServices;
using Moda.BackEnd.Common.DTO.Response;

namespace Moda.BackEnd.API.Controllers
{
    [Route("warehouse")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private IWarehouseService _warehouseService;
        public WarehouseController(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;   
        }

        [HttpGet("get-all-warehouse/{pageNumber}/{pageSize}")]
        [CacheAttribute(259200)]
        public async Task<AppActionResult> GetAllWarehouse(int pageNumber = 1, int pageSize = 10)
        {
            return await _warehouseService.GetAllWarehouse(pageNumber, pageSize);       
        }
    }
}
