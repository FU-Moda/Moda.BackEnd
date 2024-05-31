using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moda.BackEnd.API.Middlewares;
using Moda.BackEnd.Application.IServices;
using Moda.BackEnd.Common.DTO.Request;
using Moda.BackEnd.Common.DTO.Response;

namespace Moda.BackEnd.API.Controllers
{
    [Route("configuration")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private IConfigurationService _configurationService;
        public ConfigurationController(IConfigurationService configurationService)
        {
            _configurationService = configurationService; 
        }

        [HttpGet("get-all-configuration/{pageNumber}/{pageSize}")]
        public async Task<AppActionResult> GetAllConfiguration(int pageNumber = 1, int pageSize = 10)
        {
            return await _configurationService.GetAllConfiguration(pageNumber, pageSize);
        }
        [HttpPut("update-configuration")]
        public async Task<AppActionResult> UpdateConfiguration(ConfigurationDto configurationDto)
        {
            return await _configurationService.UpdateConfiguration(configurationDto);   
        }

        [RemoveCacheAtrribute("configuration")]
        public IActionResult RemoveCache()
        {
            return Ok();
        }
    }
}
