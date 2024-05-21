using Moda.BackEnd.Application.IRepositories;
using Moda.Backend.Domain.Models;
using Moda.BackEnd.Application.IServices;
using Moda.BackEnd.Common.DTO.Response;
using Moda.BackEnd.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moda.BackEnd.Common.DTO.Request;
using Moda.BackEnd.Application.Payment.PaymentRequest;
using Moda.BackEnd.Application.Payment.PaymentService;
using Microsoft.AspNetCore.Http;
using Moda.BackEnd.Domain.Enum;
using System.Transactions;
using static Humanizer.In;
using Castle.Core.Resource;

namespace Moda.BackEnd.Application.Services
{
    public class OrderService : GenericBackendService, IOrderService
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<OrderDetail> _orderDetailRepository;
        private readonly IUnitOfWork _unitOfWork;
        public OrderService(IServiceProvider serviceProvider, IUnitOfWork unitOfWork, IRepository<Order> orderRepository, IRepository<OrderDetail> orderDetailRepository) : base(serviceProvider)
        {
            _unitOfWork = unitOfWork;
            _orderDetailRepository = orderDetailRepository;
            _orderRepository = orderRepository;
        }

        public async Task<AppActionResult> CreateOrderCOD(OrderRequest orderRequest)
        {
            var result = new AppActionResult();
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var accountRepository = Resolve<IRepository<Account>>();
                    var productStockRepository = Resolve<IRepository<ProductStock>>();
                    var accountDb = await accountRepository!.GetByExpression(p => p.Id == orderRequest.AccountId);
                    if (accountDb == null)
                    {
                        result = BuildAppActionResultError(result, $"Tài khoản với {orderRequest.AccountId} không tồn tại");
                    }
                    if (string.IsNullOrEmpty(accountDb!.Address))
                    {
                        result = BuildAppActionResultError(result, "Địa chỉ của tài khoản không được để trống");
                        return result;
                    }

                    if (string.IsNullOrEmpty(accountDb!.PhoneNumber))
                    {
                        result = BuildAppActionResultError(result, "Số điện thoại của tài khoản không được để trống");
                        return result;
                    }

                    var order = new Order
                    {
                        Id = Guid.NewGuid(),
                        AccountId = accountDb!.Id,
                        Address = accountDb!.Address!,
                        OrderTime = DateTime.Now,
                        PhoneNumber = accountDb!.PhoneNumber!,
                        Status = Domain.Enum.OrderStatus.PENDING,
                        DeliveryCost = 0,
                        Total = 0
                    };

                    double total = 0;
                    foreach (var item in orderRequest.ProductStockDtos)
                    {
                        var productStock = await productStockRepository!.GetByExpression(p => p!.Id == item.Id && p.Quantity >= item.Quantity, p => p.Product!);
                        if (productStock == null)
                        {
                            result = BuildAppActionResultError(result, $"Hàng với Id {item.Id} không tồn tại hoặc sản phẩm không đủ số lượng");
                            return result;
                        }
                        var orderDetail = new OrderDetail
                        {
                            Id = Guid.NewGuid(),
                            ProductStockId = item.Id,
                            Quantity = item.Quantity,
                            OrderId = order.Id,
                        };
                        productStock.Quantity -= item.Quantity;
                        await productStockRepository.Update(productStock);

                        total += productStock.Price * item.Quantity;
                        await _orderDetailRepository.Insert(orderDetail);
                    }
                    order.Total = total;
                    if (order.Total > 250000)
                    {
                        order.DeliveryCost = 0;
                    }
                    else
                    {
                        order.DeliveryCost = 30000;
                    }
                    if (!BuildAppActionResultIsError(result))
                    {
                        await _orderRepository.Insert(order);
                        await _unitOfWork.SaveChangesAsync();
                        scope.Complete();
                        result.Messages.Add("Tạo Order thành công");
                    }
                }
                catch (Exception ex)
                {
                    result = BuildAppActionResultError(result, ex.Message);
                }
                return result;
            }
        }

        public async Task<AppActionResult> CreateOrderWithPayment(OrderRequest orderRequest, HttpContext context)
        {
            var result = new AppActionResult();
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var accountRepository = Resolve<IRepository<Account>>();
                    var productStockRepository = Resolve<IRepository<ProductStock>>();
                    var paymentGatewayService = Resolve<IPaymentGatewayService>();
                    var accountDb = await accountRepository!.GetByExpression(p => p!.Id == orderRequest.AccountId);
                    if (accountDb == null)
                    {
                        result = BuildAppActionResultError(result, $"Tài khoản với {orderRequest.AccountId} không tồn tại");
                    }
                    if (string.IsNullOrEmpty(accountDb!.Address))
                    {
                        result = BuildAppActionResultError(result, "Địa chỉ của tài khoản không được để trống");
                        return result;
                    }

                    if (string.IsNullOrEmpty(accountDb!.PhoneNumber))
                    {
                        result = BuildAppActionResultError(result, "Số điện thoại của tài khoản không được để trống");
                        return result;
                    }
                    var order = new Order
                    {
                        Id = Guid.NewGuid(),
                        AccountId = accountDb!.Id,
                        Address = accountDb!.Address!,
                        OrderTime = DateTime.Now,
                        PhoneNumber = accountDb!.PhoneNumber!,
                        Status = Domain.Enum.OrderStatus.PENDING,
                        DeliveryCost = 0,
                        Total = 0
                    };

                    double total = 0;

                    foreach (var item in orderRequest.ProductStockDtos)
                    {
                        var productStock = await productStockRepository!.GetByExpression(p => p!.Id == item.Id && p.Quantity >= item.Quantity, p => p.Product!);
                        if (productStock == null)
                        {
                            result = BuildAppActionResultError(result, $"Hàng với Id {item.Id} không tồn tại hoặc sản phẩm không đủ số lượng");
                            return result;
                        }
                        var orderDetail = new OrderDetail
                        {
                            Id = Guid.NewGuid(),
                            ProductStockId = item.Id,
                            Quantity = item.Quantity,
                            OrderId = order.Id,
                        };
                        productStock.Quantity -= item.Quantity;
                        await productStockRepository.Update(productStock);

                        total += productStock.Price * item.Quantity;
                        await _orderDetailRepository.Insert(orderDetail);
                    }
                    order.Total = total;
                    if (order.Total > 250000)
                    {
                        order.DeliveryCost = 0;
                    }
                    else
                    {
                        order.DeliveryCost = 30000;
                    }
                    if (!BuildAppActionResultIsError(result))
                    {
                        await _orderRepository.Insert(order);
                        await _unitOfWork.SaveChangesAsync();
                        scope.Complete();
                    }
                    var payment = new PaymentInformationRequest
                    {
                        AccountID = order.AccountId,
                        Amount = (double)order.Total,
                        CustomerName = $"{order.Account!.FirstName} {order.Account.LastName}",
                        OrderID = order.Id.ToString(),
                    };
                    result.Result = await paymentGatewayService!.CreatePaymentUrlVnpay(payment, context);
                }
                catch (Exception ex)
                {
                    result = BuildAppActionResultError(result, ex.Message);
                }
            }
            return result;
        }
      
        public async Task<AppActionResult> UpdatesSucessStatus(Guid orderId)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var orderRepository = Resolve<IRepository<Order>>();
                var order = await orderRepository!.GetById(orderId);
                if (order == null)
                {
                    result = BuildAppActionResultError(result, $"Không tìm thấy order với id {orderId}");
                }
                if (!BuildAppActionResultIsError(result))
                {
                    if (order != null)
                    {
                        order.Status = OrderStatus.SUCCESSFUL;
                        await _unitOfWork.SaveChangesAsync();
                    }
                }
            }
            catch (Exception e)
            {
                result = BuildAppActionResultError(result, e.Message);
            }
            return result;
        }

        public async Task<AppActionResult> GetAllOrder(int pageNumber, int pageSize)
        {
            var result = new AppActionResult();
            try
            {
                result.Result = await _orderDetailRepository.GetAllDataByExpression(null, pageNumber, pageSize, p => p.Order!.OrderTime, false, p => p.Order!);
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> GetAllOrderByAccountId(string accountId, int pageNumber, int pageSize)
        {
            var result = new AppActionResult();
            try
            {
                var accountRepository = Resolve<IRepository<Account>>();
                var accountDb = await accountRepository!.GetByExpression(p => p.Id == accountId);
                if (accountDb == null)
                {
                    result = BuildAppActionResultError(result, $"Tài khoản với {accountId} không tồn tại");
                }
                if (!BuildAppActionResultIsError(result))
                {
                    result.Result = await _orderRepository.GetAllDataByExpression(p => p.AccountId == accountId, pageNumber, pageSize, null, false, null);
                }
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> GetAllOrderByShopId(Guid shopId, int pageNumber, int pageSize)
        {
            var result = new AppActionResult();
            try
            {
                //var orderDetailDb = await _orderDetailRepository.GetAllDataByExpression(p => p.ProductStock!.Product!.ShopId == shopId, pageNumber, pageSize, null, false, p => p.Order!, p => p.ProductStock!.Product!.Shop!, p => p.Order!.Account!);

                //var groupedOrderDetails = orderDetailDb!.Items!
                //.GroupBy(od => od.OrderId)
                //.w(g => new OrderResponse
                //{
                //    Order = g.Key,
                //    OrderDetails = g.ToList()
                //})
                //.ToList();

                //// Assign grouped list to the result
                //result.Result = new PagedResult<OrderResponse>
                //{
                //    Items = groupedOrderDetails,
                //    TotalPages = orderDetailDb.TotalPages,  
                //};
                var list = new List<OrderResponse>();
                var orderDetailDb = await _orderDetailRepository.GetAllDataByExpression(p => p.ProductStock!.Product!.ShopId == shopId, pageNumber, pageSize, null, false, p => p.Order!, p => p.ProductStock!.Product!.Shop!, p => p.Order!.Account!);
                var orderResponse = orderDetailDb!.Items!.Select(a => a.Order);
                foreach (var item in orderResponse)
                {
                    var details = await _orderDetailRepository.GetAllDataByExpression(p => p.ProductStock!.Product!.ShopId == shopId, pageNumber, pageSize, null, false, p => p.Order!, p => p.ProductStock!.Product!.Shop!, p => p.Order!.Account!);
                    list.Add(new OrderResponse
                    {

                        Order = item!,
                        OrderDetails = details!.Items!
                    });
                }
                result.Result = new PagedResult<OrderResponse>
                {
                    Items = list,
                    TotalPages = orderDetailDb.TotalPages,
                };
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> GetAllOrderDetailByOrderId(Guid orderId, int pageNumber, int pageSize)
        {
            var result = new AppActionResult();
            try
            {
                var orderDb = await _orderRepository!.GetByExpression(p => p.Id == orderId);
                if (orderDb == null)
                {
                    result = BuildAppActionResultError(result, $"Đơn hàng với {orderDb} không tồn tại");
                }
                if (!BuildAppActionResultIsError(result))
                {
                    var orderDetailDb = await _orderDetailRepository.GetAllDataByExpression(p => p.OrderId == orderId, pageNumber, pageSize, null, false, p => p.ProductStock.Product);
                    OrderListResponse data = new OrderListResponse();
                    data.Order = orderDb!;
                    data.OrderDetails = orderDetailDb.Items!;
                    result.Result = data;
                }
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> GetOrderDetailsByOrderId(Guid orderId)
        {
            var result = new AppActionResult();
            try
            {
                result.Result = await _orderDetailRepository.GetByExpression(p => p!.OrderId == orderId, p => p.Order!);
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> UpdateStatus(Guid orderId, OrderStatus orderStatus)
        {
            var result = new AppActionResult();
            try
            {
                var orderDb = await _orderRepository.GetByExpression(p => p!.Id == orderId);
                if (orderDb == null)
                {
                    result = BuildAppActionResultError(result, "Đơn hàng này không tồn tại");
                }
                else
                {
                    orderDb.Status = orderStatus;
                    await _unitOfWork.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

    }
}
