using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moda.BackEnd.Application.IServices;
using Moda.BackEnd.Common.DTO.Request;
using Moda.BackEnd.Common.DTO.Response;

namespace Moda.BackEnd.API.Controllers
{
    [Route("cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService; 
        }

        [HttpGet("get-cart-items/{accountId}")]
        public async Task<AppActionResult> GetCartItems(string accountId)
        {
            return await _cartService.GetCartItems(accountId);  
        }

        [HttpPost("update-cart-detail/{accountId}/{pageNumber}/{pageSize}")]
        public async Task<AppActionResult> UpdateCartDetail(string accountId, IEnumerable<CartDetailDto> cartDetailDtos, int pageNumber = 1, int pageSize = 10)
        {
            return await _cartService.UpdateCartDetail(accountId, cartDetailDtos, pageNumber, pageSize);    
        }
    }
}
