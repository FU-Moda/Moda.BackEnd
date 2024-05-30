using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moda.BackEnd.API.Middlewares;
using Moda.BackEnd.Application.IServices;
using Moda.BackEnd.Common.DTO.Request;
using Moda.BackEnd.Common.DTO.Response;

namespace Moda.BackEnd.API.Controllers
{
    [Route("option-package")]
    [ApiController]
    public class OptionPackageController : ControllerBase
    {
        private IOptionPackageService _optionPackageService;
        public OptionPackageController(IOptionPackageService optionPackageService)
        {
            _optionPackageService = optionPackageService;   
        }

        [HttpGet("get-all-option-package/{pageNumber}/{pageSize}")]
        [CacheAttribute(259200)]
        public async Task<AppActionResult> GetAllOptionPackage(int pageNumber = 1, int pageSize = 10)
        {
            return await _optionPackageService.GetAllOptionPackage(pageNumber, pageSize);   
        }

        [HttpGet("get-option-package-by-Id/{optionPackageId}")]
        [CacheAttribute(259200)]
        public async Task<AppActionResult> GetOptionPackageById(Guid optionPackageId)
        {
            return await _optionPackageService.GetOptionPackageById(optionPackageId);   
        }

        [HttpPost("create-option-package")]
        [RemoveCacheAtrribute("option-package")]
        public async Task<AppActionResult> CreateOptionPackage(OptionPackageHistoryDto optionPackageHistoryDto)
        {
            return await _optionPackageService.CreateOptionPackage(optionPackageHistoryDto);       
        }

        [HttpPut("update-option-package")]
        [RemoveCacheAtrribute("option-package")]
        public async Task<AppActionResult> UpdateOptionPackage(Guid packageId, OptionPackageHistoryDto optionPackageHistoryDto)
        {
            return await _optionPackageService.UpdateOptionPackage(packageId, optionPackageHistoryDto);       
        }

        [HttpPost("delete-option-package")]
        [RemoveCacheAtrribute("option-package")]
        public async Task<AppActionResult> DeleteOptionPackage(Guid packageId)
        {
            return await _optionPackageService.DeleteOptionPackage(packageId);      
        }

        [RemoveCacheAtrribute("option-package")]
        [HttpGet("option-package")]
        public IActionResult RemoveCache()
        {
            return Ok();
        }
    }
}
