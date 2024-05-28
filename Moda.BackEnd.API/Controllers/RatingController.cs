using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moda.BackEnd.API.Middlewares;
using Moda.BackEnd.Application.IServices;
using Moda.BackEnd.Common.DTO.Request;
using Moda.BackEnd.Common.DTO.Response;

namespace Moda.BackEnd.API.Controllers
{
    [Route("api/rating")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private IRatingService _service;
        public RatingController(IRatingService service)
        {
            _service = service;
        }

        [HttpPost("create-rating")]
        [RemoveCacheAtrribute("rating")]
        public async Task<AppActionResult> CreateRating(CreateRatingRequest dto)
        {
            return await _service.CreateRating(dto);
        }

        [HttpPost("update-rating")]
        [RemoveCacheAtrribute("rating")]
        public async Task<AppActionResult> UpdateRating(UpdateRatingDto dto)
        {
            return await _service.UpdateRating(dto);
        }

        [HttpDelete("delete-rating")]
        [RemoveCacheAtrribute("rating")]
        public async Task<AppActionResult> DeleteRating(Guid Id)
        {
            return await _service.DeleteRating(Id);
        }
    }
}
