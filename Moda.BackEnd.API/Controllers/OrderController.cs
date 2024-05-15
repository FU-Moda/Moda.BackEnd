﻿using Microsoft.AspNetCore.Http;
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

        [HttpGet("get-all-order-by-accout-id/{accountId}/{pageNumber}/{pageSize}")]
        public async Task<AppActionResult> GetAllOrderByAccountId(string accountId, int pageNumber = 1, int pageSize = 10)
        {
            return await _orderService.GetAllOrderByAccountId(accountId, pageNumber, pageSize);
        }

        [HttpPut]
        public async Task<AppActionResult> UpdateStatus(Guid orderId, OrderStatus orderStatus)
        {
            return await _orderService.UpdateStatus(orderId, orderStatus);      
        }
    }
}
