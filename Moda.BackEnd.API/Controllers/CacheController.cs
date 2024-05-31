using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Moda.BackEnd.Application.IServices;
using Moda.BackEnd.Common.ConfigurationModel;

namespace Moda.BackEnd.API.Controllers
{
    [Route("cache")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        private readonly RedisConfiguration _cacheConfiguration;
        public readonly IResponseCacheService _responseCacheService;

        public CacheController(IDistributedCache distributedCache, RedisConfiguration cacheConfiguration, IResponseCacheService responseCacheService)
        {
            _cacheConfiguration = cacheConfiguration;
            _responseCacheService = responseCacheService;
        }

        [HttpPost("clear-cache/{pathEndPoint}")]
        public async Task<IActionResult> ClearCacheAsync(string pathEndPoint)
        {
            if (!_cacheConfiguration.Enabled)
            {
                return Ok();
            }

            var cacheKey = "/";
            await _responseCacheService.RemoveCacheResponseAsync(cacheKey);
            return Ok();
        }
    }

}
