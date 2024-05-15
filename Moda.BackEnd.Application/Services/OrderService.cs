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

        public async Task<AppActionResult> CreateOrderWithPayment(int quantity, string accountId, List<ProductStockDto> productStockDtos)
        {
            var result = new AppActionResult();
            try
            {
                var accountRepository = Resolve<IRepository<Account>>();
                var productStockRepository = Resolve<IRepository<ProductStock>>();
                var accountDb = accountRepository!.GetByExpression(p => p!.Id == accountId);
                if (accountDb == null)
                {
                    result = BuildAppActionResultError(result, $"Tài khoản với {accountId} không tồn tại");
                }
                foreach (var item in  productStockDtos)
                {

                }
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;  
        }

        public async Task<AppActionResult> GetAllOrder(int pageNumber, int pageSize)
        {
            var result = new AppActionResult();
            try
            {
                result.Result = await _orderDetailRepository.GetAllDataByExpression(null, pageNumber, pageSize, null, false, p => p.Order!);
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
                    result = BuildAppActionResultError(result , $"Tài khoản với {accountId} không tồn tại");
                }
                result.Result  = await _orderDetailRepository.GetAllDataByExpression(p => p.Order.AccountId == accountId, pageNumber, pageSize, null, false, p => p.Order!);
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public Task<AppActionResult> UpdateStatus(string orderId, int status)
        {
            throw new NotImplementedException();
        }
    }
}
