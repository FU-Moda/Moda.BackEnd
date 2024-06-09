using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moda.BackEnd.API.Middlewares;
using Moda.BackEnd.Application.IServices;
using Moda.BackEnd.Application.Payment.PaymentResponse;
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
        [RemoveCacheAtrribute("option-package")]
        public async Task<AppActionResult> CreateOrderWithPayment( OrderRequest orderRequest)
        {
           return await _orderService.CreateOrderWithPayment(orderRequest, HttpContext);     
        }
        [HttpPost("create-order-cod")]
        [RemoveCacheAtrribute("option-package")]
        public async Task<AppActionResult> CreateOrderCOD(OrderRequest orderRequest) 
        {
            return await _orderService.CreateOrderCOD(orderRequest);
        }
        [HttpGet("VNPayIpn")]
        public async Task<IActionResult> VNPayIPN()
        {
            try
            {
                var response = new VNPayResponseDto
                {
                    PaymentMethod = Request.Query["vnp_BankCode"],
                    OrderDescription = Request.Query["vnp_OrderInfo"],
                    OrderId = Request.Query["vnp_TxnRef"],
                    PaymentId = Request.Query["vnp_TransactionNo"],
                    TransactionId = Request.Query["vnp_TransactionNo"],
                    Token = Request.Query["vnp_SecureHash"],
                    VnPayResponseCode = Request.Query["vnp_ResponseCode"],
                    PayDate = Request.Query["vnp_PayDate"],
                    Amount = Request.Query["vnp_Amount"],
                    Success = true
                };

                if (response.VnPayResponseCode == "00")
                {
                    var orderId = response.OrderId.ToString().Split(" ");
                    //await _orderService.UpdatesSucessStatus(Guid.Parse(orderId[0]));
                   
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok(new
            {
                RspCode = "00",
                Message = "Confirm Success"
            });
        }

        [HttpGet("get-all-order/{pageNumber}/{pageSize}")]
        [CacheAttribute(259200)]
        public async Task<AppActionResult> GetAllOrder(int pageNumber = 1, int pageSize = 10)
        {
            return await _orderService.GetAllOrder(pageNumber, pageSize);       
        }

        [HttpGet("get-all-order-by-status/{pageNumber}/{pageSize}")]
        [CacheAttribute(259200)]
        public async Task<AppActionResult> GetAllOrderByStatus(OrderStatus orderStatus,int pageNumber = 1, int pageSize = 10)
        {
            return await _orderService.GetAllOrderByStatus(orderStatus, pageNumber, pageSize);
        }

        [HttpGet("get-shop-order-by-status/{shopId}/{pageNumber}/{pageSize}")]
        [CacheAttribute(259200)]
        public async Task<AppActionResult> GetShopOrderByStatus(Guid shopId ,OrderStatus orderStatus, int pageNumber = 1, int pageSize = 10)
        {
            return await _orderService.GetShopOrderByStatus(shopId, orderStatus, pageNumber, pageSize);
        }

        [HttpGet("get-all-order-by-account-id/{accountId}/{pageNumber}/{pageSize}")]
        [CacheAttribute(259200)]
        public async Task<AppActionResult> GetAllOrderByAccountId(string accountId, int pageNumber = 1, int pageSize = 10)
        {
            return await _orderService.GetAllOrderByAccountId(accountId, pageNumber, pageSize);
        }

        [HttpGet("get-all-order-detail-by-order-id/{orderId}/{pageNumber}/{pageSize}")]
        [CacheAttribute(259200)]
        public async Task<AppActionResult> GetAllOrderDetailByOrderId(Guid orderId, int pageNumber = 1, int pageSize = 10)
        {
            return await _orderService.GetAllOrderDetailByOrderId(orderId, pageNumber, pageSize);
        }

        [HttpPut("update-status")]
        [RemoveCacheAtrribute("order")]
        public async Task<AppActionResult> UpdateStatus(Guid orderId)
        {
            return await _orderService.UpdateStatus(orderId, true);      
        }

        [HttpGet("get-all-order-by-shop-id/{shopId}")]
        [CacheAttribute(259200)]
        public async Task<AppActionResult> GetAllOrderByShopId(Guid shopId, int pageNumber = 1, int pageSize = 10)
        {
            return await _orderService.GetAllOrderByShopId(shopId, pageNumber, pageSize);
        }

        [HttpGet("get-order-details-by-order-id/{orderId}")]
        [CacheAttribute(259200)]
        public async Task<AppActionResult> GetOrderDetailsByOrderId(Guid orderId)
        {
            return await _orderService.GetOrderDetailsByOrderId(orderId);
        }

        [RemoveCacheAtrribute("order")]
        [HttpGet("remove-cache")]
        public IActionResult RemoveCache()
        {
            return Ok();
        }
    }
}
