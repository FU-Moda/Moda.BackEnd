using Microsoft.AspNetCore.Http;
using Moda.BackEnd.Common.DTO.Request;
using Moda.BackEnd.Common.DTO.Response;
using Moda.BackEnd.Domain.Enum;
using Moda.BackEnd.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Application.IServices
{
    public interface IOrderService
    {
        Task<AppActionResult> CreateOrderWithPayment( OrderRequest orderRequest, HttpContext context);
        Task<AppActionResult> CreateOrderCOD( OrderRequest orderRequest);
        Task<AppActionResult> UpdatesSucessStatus(Guid orderId);
        Task<AppActionResult> GetAllOrder(int pageNumber, int pageSize);
        Task<AppActionResult> GetAllOrderByAccountId(string accountId, int pageNumber, int pageSize);
        Task<AppActionResult> GetAllOrderDetailByOrderId(Guid orderId, int pageNumber, int pageSize);
        Task<AppActionResult> UpdateStatus(Guid orderId, OrderStatus orderStatus);
        Task<AppActionResult> GetAllOrderByShopId(Guid shopId, int pageNumber, int pageSize);
        Task<AppActionResult> GetOrderDetailsByOrderId(Guid orderId);
    }
}
