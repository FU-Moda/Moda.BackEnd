using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moda.BackEnd.Application.IServices;
using Moda.BackEnd.Application.Services;
using Moda.BackEnd.Common.DTO.Request;
using Moda.BackEnd.Common.DTO.Response;
using Moda.BackEnd.Domain.Enum;

namespace Moda.BackEnd.API.Controllers
{
    [Route("order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;   
        }

        [HttpPost("create-order-with-payment")]
        public async Task<AppActionResult> CreateOrderWithPayment( OrderRequest orderRequest)
        {
           return await _orderService.CreateOrderWithPayment(orderRequest, HttpContext);     
        }

        [HttpGet("get-all-order/{pageNumber}/{pageSize}")]
        public async Task<AppActionResult> GetAllOrder(int pageNumber = 1, int pageSize = 10)
        {
            return await _orderService.GetAllOrder(pageNumber, pageSize);       
        }

        [HttpGet("get-all-order-by-account-id/{accountId}/{pageNumber}/{pageSize}")]
        public async Task<AppActionResult> GetAllOrderByAccountId(string accountId, int pageNumber = 1, int pageSize = 10)
        {
            return await _orderService.GetAllOrderByAccountId(accountId, pageNumber, pageSize);
        }

        [HttpGet("get-all-order-detail-by-order-id/{orderId}/{pageNumber}/{pageSize}")]
        public async Task<AppActionResult> GetAllOrderDetailByOrderId(Guid orderId, int pageNumber = 1, int pageSize = 10)
        {
            return await _orderService.GetAllOrderDetailByOrderId(orderId, pageNumber, pageSize);
        }

        [HttpPut("update-status")]
        public async Task<AppActionResult> UpdateStatus(Guid orderId, OrderStatus orderStatus)
        {
            return await _orderService.UpdateStatus(orderId, orderStatus);      
        }

        [HttpGet("get-all-order-by-shop-id/{shopId}")]
        public async Task<AppActionResult> GetAllOrderByShopId(Guid shopId, int pageNumber = 1, int pageSize = 10)
        {
            return await _orderService.GetAllOrderByShopId(shopId, pageNumber, pageSize);
        }

        [HttpGet("get-order-details-by-order-id/{orderId}")]
        public async Task<AppActionResult> GetOrderDetailsByOrderId(Guid orderId)
        {
            return await _orderService.GetOrderDetailsByOrderId(orderId);
        }
    }
}
