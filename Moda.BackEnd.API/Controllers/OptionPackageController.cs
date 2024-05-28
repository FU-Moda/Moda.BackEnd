using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<AppActionResult> GetAllOptionPackage(int pageNumber = 10, int pageSize = 1)
        {
            return await _optionPackageService.GetAllOptionPackage(pageNumber, pageSize);   
        }

        [HttpGet("get-option-package-by-Id/{optionPackageId}")]
        public async Task<AppActionResult> GetOptionPackageById(Guid optionPackageId)
        {
            return await _optionPackageService.GetOptionPackageById(optionPackageId);   
        }

        [HttpPost("create-option-package")]
        public async Task<AppActionResult> CreateOptionPackage(OptionPackageDto optionPackageDto)
        {
            return await _optionPackageService.CreateOptionPackage(optionPackageDto);       
        }

        [HttpPut("update-option-package")]
        public async Task<AppActionResult> UpdateOptionPackage(Guid packageId, OptionPackageDto optionPackageDto)
        {
            return await _optionPackageService.UpdateOptionPackage(packageId, optionPackageDto);       
        }

        [HttpPost("delete-option-package")]
        public async Task<AppActionResult> DeleteOptionPackage(Guid packageId)
        {
            return await _optionPackageService.DeleteOptionPackage(packageId);      
        }
    }
}
