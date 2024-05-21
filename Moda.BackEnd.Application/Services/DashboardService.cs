﻿using AutoMapper;
using Moda.BackEnd.Application.IRepositories;
using Moda.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moda.BackEnd.Application.IServices;
using Moda.BackEnd.Common.DTO.Response;
using Moda.BackEnd.Common.DTO.Request;
using Moda.BackEnd.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Bcpg;
using Moda.BackEnd.Common.Utils;
using System.Net.Http.Headers;

namespace Moda.BackEnd.Application.Services
{
    public class DashboardService: GenericBackendService, IDashBoardService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<OrderDetail> _orderDetailRepository;
        private readonly IRepository<ProductTag> _productTagRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Account> _accountRepository;
        private readonly IRepository<IdentityRole> _roleRepository;
        private readonly IRepository<IdentityUserRole<string>> _userRoleRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DashboardService(
            IServiceProvider serviceProvider, 
            IMapper mapper, 
            IRepository<Order> orderRepository, 
            IRepository<OrderDetail> orderDetailRepository,
            IRepository<ProductTag> productTagRepository,
            IRepository<Product> productRepository,
            IRepository<Account> accountRepository,
            IRepository<IdentityRole> roleRepository,
            IRepository<IdentityUserRole<string>> userRoleRepository,
            IUnitOfWork unitOfWork
            ) : base(serviceProvider)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _productTagRepository = productTagRepository;
            _productRepository = productRepository;
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AppActionResult> GetProductReport(int timePeriod, Guid? ShopId = null)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                //if (timePeriod > 3)
                //{
                //    if (ShopId != null)
                //    {
                //        var orderDetail = await _orderDetailRepository.GetAllDataByExpression(o => o.Order.OrderTime.Year == timePeriod && o.ProductStock.Product.ShopId == ShopId, 0, 0, null, false, o => o.ProductStock.Product);
                //        if (orderDetail.Items != null && orderDetail.Items.Count > 0)
                //        {
                //            var productRevenue = orderDetail.Items.GroupBy(s => s.ProductStock.ProductId).ToDictionary(s => s.Key, s => s.Sum(s => s.Quantity * s.ProductStock.Price));
                            
                //            result.Result = GetProductReportResponse(productRevenue.OrderByDescending(k => k.Value).Take(10).ToDictionary(k => k.Key, k => k.Value));
                //        }
                //    } 
                //    else
                //    {
                //        var orderDetail = await _orderDetailRepository.GetAllDataByExpression(o => o.Order.OrderTime.Year == timePeriod, 0, 0, null, false, o => o.ProductStock.Product);
                //        if(orderDetail.Items != null && orderDetail.Items.Count > 0)
                //        {
                //            var productRevenue = orderDetail.Items.GroupBy(s => s.ProductStock.ProductId).ToDictionary(s => s.Key, s => s.Sum(s => s.Quantity * s.ProductStock.Price));
                //            result.Result = GetProductReportResponse(productRevenue.OrderByDescending(k => k.Value).Take(10).ToDictionary(k => k.Key, k => k.Value));
                //        }
                //    }
                //} else if (timePeriod == 0)
                //{
                //    if (ShopId != null)
                //    {
                //        var orderDetail = await _orderDetailRepository.GetAllDataByExpression(o => o.Order.OrderTime.AddDays(7) >= DateTime.Now && o.ProductStock.Product.ShopId == ShopId, 0, 0, null, false, o => o.ProductStock.Product);
                //        if (orderDetail.Items != null && orderDetail.Items.Count > 0)
                //        {
                //            var productRevenue = orderDetail.Items.GroupBy(s => s.ProductStock.ProductId).ToDictionary(s => s.Key, s => s.Sum(s => s.Quantity * s.ProductStock.Price));
                //            result.Result = GetProductReportResponse(productRevenue.OrderByDescending(k => k.Value).Take(10).ToDictionary(k => k.Key, k => k.Value));
                //        }
                //    }
                //    else
                //    {
                //        var orderDetail = await _orderDetailRepository.GetAllDataByExpression(o => o.Order.OrderTime.AddDays(7) >= DateTime.UtcNow, 0, 0, null, false, o => o.ProductStock.Product);
                //        if (orderDetail.Items != null && orderDetail.Items.Count > 0)
                //        {
                //            var productRevenue = orderDetail.Items.GroupBy(s => s.ProductStock.ProductId).ToDictionary(s => s.Key, s => s.Sum(s => s.Quantity * s.ProductStock.Price));
                //            result.Result = GetProductReportResponse(productRevenue.OrderByDescending(k => k.Value).Take(10).ToDictionary(k => k.Key, k => k.Value));
                //        }
                //    }
                //}
                //else if (timePeriod == 1)
                //{
                //    if (ShopId != null)
                //    {
                //        var orderDetail = await _orderDetailRepository.GetAllDataByExpression(o => o.Order.OrderTime.AddMonths(1) >= DateTime.Now && o.ProductStock.Product.ShopId == ShopId, 0, 0, null, false, o => o.ProductStock.Product);
                //        if (orderDetail.Items != null && orderDetail.Items.Count > 0)
                //        {
                //            var productRevenue = orderDetail.Items.GroupBy(s => s.ProductStock.ProductId).ToDictionary(s => s.Key, s => s.Sum(s => s.Quantity * s.ProductStock.Price));
                //            result.Result = GetProductReportResponse(productRevenue.OrderByDescending(k => k.Value).Take(10).ToDictionary(k => k.Key, k => k.Value));
                //        }
                //    }
                //    else
                //    {
                //        var orderDetail = await _orderDetailRepository.GetAllDataByExpression(o => o.Order.OrderTime.AddMonths(1) >= DateTime.UtcNow, 0, 0, null, false, o => o.ProductStock.Product);
                //        if (orderDetail.Items != null && orderDetail.Items.Count > 0)
                //        {
                //            var productRevenue = orderDetail.Items.GroupBy(s => s.ProductStock.ProductId).ToDictionary(s => s.Key, s => s.Sum(s => s.Quantity * s.ProductStock.Price));
                //            result.Result = GetProductReportResponse(productRevenue.OrderByDescending(k => k.Value).Take(10).ToDictionary(k => k.Key, k => k.Value));
                //        }
                //    }
                //}
                //else if (timePeriod == 2)
                //{
                //    if (ShopId != null)
                //    {
                //        var orderDetail = await _orderDetailRepository.GetAllDataByExpression(o => o.ProductStock.Product.ShopId == ShopId && o.Order.OrderTime.AddMonths(6) >= DateTime.UtcNow, 0, 0, null, false, o => o.ProductStock.Product);
                //        if (orderDetail.Items != null && orderDetail.Items.Count > 0)
                //        {
                //            var productRevenue = orderDetail.Items.GroupBy(s => s.ProductStock.ProductId).ToDictionary(s => s.Key, s => s.Sum(s => s.Quantity * s.ProductStock.Price));
                //            result.Result = GetProductReportResponse(productRevenue.OrderByDescending(k => k.Value).Take(10).ToDictionary(k => k.Key, k => k.Value));
                //        }
                //    }
                //    else
                //    {
                //        var orderDetail = await _orderDetailRepository.GetAllDataByExpression(o => o.Order.OrderTime.AddMonths(6) >= DateTime.UtcNow, 0, 0, null, false, o => o.ProductStock.Product);
                //        if (orderDetail.Items != null && orderDetail.Items.Count > 0)
                //        {
                //            var productRevenue = orderDetail.Items.GroupBy(s => s.ProductStock.ProductId).ToDictionary(s => s.Key, s => s.Sum(s => s.Quantity * s.ProductStock.Price));
                //            result.Result = GetProductReportResponse(productRevenue.OrderByDescending(k => k.Value).Take(10).ToDictionary(k => k.Key, k => k.Value));
                //        }
                //    }
                //}
                //else 
                //{
                //    if (ShopId != null)
                //    {
                //        var orderDetail = await _orderDetailRepository.GetAllDataByExpression(o => o.ProductStock.Product.ShopId == ShopId && o.Order.OrderTime.AddMonths(12) >= DateTime.UtcNow, 0, 0, null, false, o => o.ProductStock.Product);
                //        if (orderDetail.Items != null && orderDetail.Items.Count > 0)
                //        {
                //            var productRevenue = orderDetail.Items.GroupBy(s => s.ProductStock.ProductId).ToDictionary(s => s.Key, s => s.Sum(s => s.Quantity * s.ProductStock.Price));
                //            result.Result = GetProductReportResponse(productRevenue.OrderByDescending(k => k.Value).Take(10).ToDictionary(k => k.Key, k => k.Value));
                //        }
                //    }
                //    else
                //    {
                //        var orderDetail = await _orderDetailRepository.GetAllDataByExpression(o => o.Order.OrderTime.AddMonths(12) >= DateTime.UtcNow, 0, 0, null, false, o => o.ProductStock.Product);
                //        if (orderDetail.Items != null && orderDetail.Items.Count > 0)
                //        {
                //            var productRevenue = orderDetail.Items.GroupBy(s => s.ProductStock.ProductId).ToDictionary(s => s.Key, s => s.Sum(s => s.Quantity * s.ProductStock.Price));
                //            result.Result = GetProductReportResponse((Dictionary<Guid, double>)productRevenue.OrderByDescending(k => k.Value).Take(10).ToDictionary(k => k.Key, k => k.Value));
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<ProductReportResponse> GetProductReportResponse(Dictionary<Guid, double> products)
        {
            ProductReportResponse data = new ProductReportResponse();
            if(products.Count > 0)
            {
                List<Tag> tags = new List<Tag>();
                foreach(var kvp in products)
                {
                    
                    var productDb = await _productRepository.GetById(kvp.Key);
                    if(productDb != null)
                    {
                        data.productReports.Add(new ProductReport
                        {
                            Product = productDb,
                            total = kvp.Value,
                        });
                        var productTag = await _productTagRepository.GetAllDataByExpression(p => p.ProductId == kvp.Key, 0, 0, null, false, p => p.Tag);
                        tags.AddRange(productTag.Items.Select(p => p.Tag)!);
                    }
                }
                data.tags = tags.DistinctBy(t => t.Id).Select(t => t.Name).ToList();
            }
            return data;
        }

        public async Task<AppActionResult> GetRevenueReport(int timePeriod, Guid? ShopId = null)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                RevenueResponse data = new RevenueResponse();
                var orderDb = new PagedResult<Order>();
                // 0: week, 1: month, 2: 6 months, 3: last year, 2022, 2023
                if(timePeriod > 3)
                {
                    if(ShopId != null)
                    {
                        var orderDetailFromShop = await _orderDetailRepository.GetAllDataByExpression(o => o.ProductStock.Product.ShopId == ShopId, 0, 0, null, false, o => o.Order);
                        if(orderDetailFromShop.Items != null && orderDetailFromShop.Items.Count > 0)
                        {
                            List<Order> orders = orderDetailFromShop.Items.DistinctBy(o => o.OrderId).Select(o => o.Order).ToList()!;
                            result.Result = GetRevenueResponse(orders.Where(o => o.OrderTime.Year == timePeriod).ToList());
                        }
                    } else
                    {
                        orderDb = await _orderRepository.GetAllDataByExpression(o => o.OrderTime.Year == timePeriod, 0, 0, null, false, null);
                        if(orderDb.Items != null && orderDb.Items.Count > 0)
                        {
                            result.Result = GetRevenueResponse(orderDb.Items!);
                        }
                    }
                } else if(timePeriod == 0 )
                {
                    if (ShopId != null)
                    {
                        var orderDetailFromShop = await _orderDetailRepository.GetAllDataByExpression(o => o.ProductStock.Product.ShopId == ShopId, 0, 0, null, false, o => o.Order);
                        if (orderDetailFromShop.Items != null && orderDetailFromShop.Items.Count > 0)
                        {
                            List<Order> orders = orderDetailFromShop.Items.DistinctBy(o => o.OrderId).Select(o => o.Order).ToList()!;
                            result.Result = GetRevenueResponse(orders.Where(o => o.OrderTime.AddDays(7) >= DateTime.UtcNow).ToList());
                        }
                    }
                    else
                    {
                        orderDb = await _orderRepository.GetAllDataByExpression(o => o.OrderTime.AddDays(7) >= DateTime.UtcNow, 0, 0, null, false, null);
                        if (orderDb.Items != null && orderDb.Items.Count > 0)
                        {
                            result.Result = GetRevenueResponse(orderDb.Items!);
                        }
                    }
                }
                else if (timePeriod == 1)
                {
                    if (ShopId != null)
                    {
                        var orderDetailFromShop = await _orderDetailRepository.GetAllDataByExpression(o => o.ProductStock.Product.ShopId == ShopId, 0, 0, null, false, o => o.Order);
                        if (orderDetailFromShop.Items != null && orderDetailFromShop.Items.Count > 0)
                        {
                            List<Order> orders = orderDetailFromShop.Items.DistinctBy(o => o.OrderId).Select(o => o.Order).ToList()!;
                            result.Result = GetRevenueResponse(orders.Where(o => o.OrderTime.AddMonths(1) >= DateTime.UtcNow).ToList());
                        }
                    }
                    else
                    {
                        orderDb = await _orderRepository.GetAllDataByExpression(o => o.OrderTime.AddMonths(1) >= DateTime.UtcNow, 0, 0, null, false, null);
                        if (orderDb.Items != null && orderDb.Items.Count > 0)
                        {
                            result.Result = GetRevenueResponse(orderDb.Items!);
                        }
                    }
                }
                else if (timePeriod == 0)
                {
                    if (ShopId != null)
                    {
                        var orderDetailFromShop = await _orderDetailRepository.GetAllDataByExpression(o => o.ProductStock.Product.ShopId == ShopId, 0, 0, null, false, o => o.Order);
                        if (orderDetailFromShop.Items != null && orderDetailFromShop.Items.Count > 0)
                        {
                            List<Order> orders = orderDetailFromShop.Items.DistinctBy(o => o.OrderId).Select(o => o.Order).ToList()!;
                            result.Result = GetRevenueResponse(orders.Where(o => o.OrderTime.AddMonths(6) >= DateTime.UtcNow).ToList());
                        }
                    }
                    else
                    {
                        orderDb = await _orderRepository.GetAllDataByExpression(o => o.OrderTime.AddMonths(6) >= DateTime.UtcNow, 0, 0, null, false, null);
                        if (orderDb.Items != null && orderDb.Items.Count > 0)
                        {
                            result.Result = GetRevenueResponse(orderDb.Items!);
                        }
                    }
                } else
                {
                     if (ShopId != null)
                        {
                            var orderDetailFromShop = await _orderDetailRepository.GetAllDataByExpression(o => o.ProductStock.Product.ShopId == ShopId, 0, 0, null, false, o => o.Order);
                            if (orderDetailFromShop.Items != null && orderDetailFromShop.Items.Count > 0)
                            {
                                List<Order> orders = orderDetailFromShop.Items.DistinctBy(o => o.OrderId).Select(o => o.Order).ToList()!;
                                result.Result = GetRevenueResponse(orders.Where(o => o.OrderTime.AddMonths(12) >= DateTime.UtcNow).ToList());
                            }
                        }
                        else
                        {
                            orderDb = await _orderRepository.GetAllDataByExpression(o => o.OrderTime.AddMonths(12) >= DateTime.UtcNow, 0, 0, null, false, null);
                            if (orderDb.Items != null && orderDb.Items.Count > 0)
                            {
                                result.Result = GetRevenueResponse(orderDb.Items!);
                            }
                        }
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public RevenueResponse GetRevenueResponse(List<Order> orders)
        {
            RevenueResponse data = new RevenueResponse();
            if(orders.Count > 0)
            {
                data.NumOfOrder = orders.Count;
                data.Total = orders.Sum(s => s.Total);
                data.MinOrderValue = orders.MinBy(o => o.Total)!.Total;
                data.MaxOrderValue = orders.MaxBy(o => o.Total)!.Total;
                data.AvarageOrderValue = orders.Average(s => s.Total);
            }
            return data;
        }

        public async Task<AppActionResult> GetUserReport(int timePeriod, Guid? ShopId = null)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                UserReportResponse data = new UserReportResponse();
                if(timePeriod > 3)
                {
                    if(ShopId != null)
                    {
                        var toExcludeOrder = await _orderDetailRepository.GetAllDataByExpression(o => o.Order.OrderTime.Year != timePeriod && o.ProductStock.Product.ShopId == ShopId, 0, 0, null, false, null);
                        var totalOrder = await _orderDetailRepository.GetAllDataByExpression(o => o.ProductStock.Product.ShopId == ShopId, 0, 0, null, false, null);
                        data.totalCustomer = totalOrder.Items.DistinctBy(o => o.Order.AccountId).Count();
                        data.newCustomer = data.totalCustomer - toExcludeOrder.Items.DistinctBy(o => o.Order.AccountId).Count();
                    }
                    else
                    {
                        var toExcludeOrder = await _orderRepository.GetAllDataByExpression(o => o.OrderTime.Year != timePeriod, 0, 0, null, false, null);
                        string AdminId = (await _roleRepository.GetByExpression(u => u.NormalizedName.ToLower().Equals("admin")))!.Id;
                        string staffId = (await _roleRepository.GetByExpression(u => u.NormalizedName.ToLower().Equals("staff")))!.Id;
                        string customerId = (await _roleRepository.GetByExpression(u => u.NormalizedName.ToLower().Equals("customer")))!.Id;
                        HashSet<string> accountIds = new HashSet<string>();
                        var userRoleAccountDb = await _userRoleRepository.GetAllDataByExpression(u => u.RoleId == AdminId, 0, 0, null, false, null);
                        data.totalAdmin = userRoleAccountDb.Items.Count();
                        userRoleAccountDb.Items.ForEach(o => accountIds.Add(o.UserId));

                        userRoleAccountDb = await _userRoleRepository.GetAllDataByExpression(u => u.RoleId == staffId && !accountIds.Contains(u.UserId), 0, 0, null, false, null);
                        data.totalStaff = userRoleAccountDb.Items.Count();
                        userRoleAccountDb.Items.ForEach(o => accountIds.Add(o.UserId));

                        userRoleAccountDb = await _userRoleRepository.GetAllDataByExpression(u => u.RoleId == customerId && !accountIds.Contains(u.UserId), 0, 0, null, false, null);
                        data.totalCustomer = userRoleAccountDb.Items.Count();
                        data.newCustomer = data.totalCustomer - toExcludeOrder.Items.DistinctBy(o => o.AccountId).Count();
                    }
                } else if(timePeriod == 0)
                {
                    if (ShopId != null)
                    {
                        var toExcludeOrder = await _orderDetailRepository.GetAllDataByExpression(o => o.Order.OrderTime.AddDays(7) < DateTime.Now && o.ProductStock.Product.ShopId == ShopId, 0, 0, null, false, null);
                        var totalOrder = await _orderDetailRepository.GetAllDataByExpression(o => o.ProductStock.Product.ShopId == ShopId, 0, 0, null, false, null);
                        data.totalCustomer = totalOrder.Items.DistinctBy(o => o.Order.AccountId).Count();
                        data.newCustomer = data.totalCustomer - toExcludeOrder.Items.DistinctBy(o => o.Order.AccountId).Count();
                    }
                    else
                    {
                        var toExcludeOrder = await _orderRepository.GetAllDataByExpression(o => o.OrderTime.AddDays(7) < DateTime.Now, 0, 0, null, false, null);
                        string AdminId = (await _roleRepository.GetByExpression(u => u.NormalizedName.ToLower().Equals("admin")))!.Id;
                        string staffId = (await _roleRepository.GetByExpression(u => u.NormalizedName.ToLower().Equals("staff")))!.Id;
                        string customerId = (await _roleRepository.GetByExpression(u => u.NormalizedName.ToLower().Equals("customer")))!.Id;
                        HashSet<string> accountIds = new HashSet<string>();
                        var userRoleAccountDb = await _userRoleRepository.GetAllDataByExpression(u => u.RoleId == AdminId, 0, 0, null, false, null);
                        data.totalAdmin = userRoleAccountDb.Items.Count();
                        userRoleAccountDb.Items.ForEach(o => accountIds.Add(o.UserId));

                        userRoleAccountDb = await _userRoleRepository.GetAllDataByExpression(u => u.RoleId == staffId && !accountIds.Contains(u.UserId), 0, 0, null, false, null);
                        data.totalStaff = userRoleAccountDb.Items.Count();
                        userRoleAccountDb.Items.ForEach(o => accountIds.Add(o.UserId));

                        userRoleAccountDb = await _userRoleRepository.GetAllDataByExpression(u => u.RoleId == customerId && !accountIds.Contains(u.UserId), 0, 0, null, false, null);
                        data.totalCustomer = userRoleAccountDb.Items.Count();
                        data.newCustomer = data.totalCustomer - toExcludeOrder.Items.DistinctBy(o => o.AccountId).Count();
                    }
                }
                else if (timePeriod == 1)
                {
                    if (ShopId != null)
                    {
                        var toExcludeOrder = await _orderDetailRepository.GetAllDataByExpression(o => o.Order.OrderTime.AddMonths(1) < DateTime.Now && o.ProductStock.Product.ShopId == ShopId, 0, 0, null, false, null);
                        var totalOrder = await _orderDetailRepository.GetAllDataByExpression(o => o.ProductStock.Product.ShopId == ShopId, 0, 0, null, false, null);
                        data.totalCustomer = totalOrder.Items.DistinctBy(o => o.Order.AccountId).Count();
                        data.newCustomer = data.totalCustomer - toExcludeOrder.Items.DistinctBy(o => o.Order.AccountId).Count();
                    }
                    else
                    {
                        var toExcludeOrder = await _orderRepository.GetAllDataByExpression(o => o.OrderTime.AddMonths(1) < DateTime.Now, 0, 0, null, false, null);
                        string AdminId = (await _roleRepository.GetByExpression(u => u.NormalizedName.ToLower().Equals("admin")))!.Id;
                        string staffId = (await _roleRepository.GetByExpression(u => u.NormalizedName.ToLower().Equals("staff")))!.Id;
                        string customerId = (await _roleRepository.GetByExpression(u => u.NormalizedName.ToLower().Equals("customer")))!.Id;
                        HashSet<string> accountIds = new HashSet<string>();
                        var userRoleAccountDb = await _userRoleRepository.GetAllDataByExpression(u => u.RoleId == AdminId, 0, 0, null, false, null);
                        data.totalAdmin = userRoleAccountDb.Items.Count();
                        userRoleAccountDb.Items.ForEach(o => accountIds.Add(o.UserId));

                        userRoleAccountDb = await _userRoleRepository.GetAllDataByExpression(u => u.RoleId == staffId && !accountIds.Contains(u.UserId), 0, 0, null, false, null);
                        data.totalStaff = userRoleAccountDb.Items.Count();
                        userRoleAccountDb.Items.ForEach(o => accountIds.Add(o.UserId));

                        userRoleAccountDb = await _userRoleRepository.GetAllDataByExpression(u => u.RoleId == customerId && !accountIds.Contains(u.UserId), 0, 0, null, false, null);
                        data.totalCustomer = userRoleAccountDb.Items.Count();
                        data.newCustomer = data.totalCustomer - toExcludeOrder.Items.DistinctBy(o => o.AccountId).Count();
                    }
                }
                else if (timePeriod == 2)
                {
                    if (ShopId != null)
                    {
                        var toExcludeOrder = await _orderDetailRepository.GetAllDataByExpression(o => o.Order.OrderTime.AddMonths(6) < DateTime.Now && o.ProductStock.Product.ShopId == ShopId, 0, 0, null, false, null);
                        var totalOrder = await _orderDetailRepository.GetAllDataByExpression(o => o.ProductStock.Product.ShopId == ShopId, 0, 0, null, false, null);
                        data.totalCustomer = totalOrder.Items.DistinctBy(o => o.Order.AccountId).Count();
                        data.newCustomer = data.totalCustomer - toExcludeOrder.Items.DistinctBy(o => o.Order.AccountId).Count();
                    }
                    else
                    {
                        var toExcludeOrder = await _orderRepository.GetAllDataByExpression(o => o.OrderTime.AddMonths(6) < DateTime.Now, 0, 0, null, false, null);
                        string AdminId = (await _roleRepository.GetByExpression(u => u.NormalizedName.ToLower().Equals("admin")))!.Id;
                        string staffId = (await _roleRepository.GetByExpression(u => u.NormalizedName.ToLower().Equals("staff")))!.Id;
                        string customerId = (await _roleRepository.GetByExpression(u => u.NormalizedName.ToLower().Equals("customer")))!.Id;
                        HashSet<string> accountIds = new HashSet<string>();
                        var userRoleAccountDb = await _userRoleRepository.GetAllDataByExpression(u => u.RoleId == AdminId, 0, 0, null, false, null);
                        data.totalAdmin = userRoleAccountDb.Items.Count();
                        userRoleAccountDb.Items.ForEach(o => accountIds.Add(o.UserId));

                        userRoleAccountDb = await _userRoleRepository.GetAllDataByExpression(u => u.RoleId == staffId && !accountIds.Contains(u.UserId), 0, 0, null, false, null);
                        data.totalStaff = userRoleAccountDb.Items.Count();
                        userRoleAccountDb.Items.ForEach(o => accountIds.Add(o.UserId));

                        userRoleAccountDb = await _userRoleRepository.GetAllDataByExpression(u => u.RoleId == customerId && !accountIds.Contains(u.UserId), 0, 0, null, false, null);
                        data.totalCustomer = userRoleAccountDb.Items.Count();
                        data.newCustomer = data.totalCustomer - toExcludeOrder.Items.DistinctBy(o => o.AccountId).Count();
                    }
                }
                else
                {
                    if (ShopId != null)
                    {
                        var toExcludeOrder = await _orderDetailRepository.GetAllDataByExpression(o => o.Order.OrderTime.AddYears(1) < DateTime.Now && o.ProductStock.Product.ShopId == ShopId, 0, 0, null, false, null);
                        data.newCustomer = data.totalCustomer - toExcludeOrder.Items.DistinctBy(o => o.Order.AccountId).Count();
                    }
                    else
                    {
                        var toExcludeOrder = await _orderRepository.GetAllDataByExpression(o => o.OrderTime.AddYears(1) < DateTime.Now, 0, 0, null, false, null);
                        var totalOrder = await _orderRepository.GetAllDataByExpression(null, 0, 0, null, false, null);
                        string AdminId = (await _roleRepository.GetByExpression(u => u.NormalizedName.ToLower().Equals("admin")))!.Id;
                        string staffId = (await _roleRepository.GetByExpression(u => u.NormalizedName.ToLower().Equals("staff")))!.Id;
                        string customerId = (await _roleRepository.GetByExpression(u => u.NormalizedName.ToLower().Equals("customer")))!.Id;
                        HashSet<string> accountIds = new HashSet<string>();
                        var userRoleAccountDb = await _userRoleRepository.GetAllDataByExpression(u => u.RoleId == AdminId, 0, 0, null, false, null);
                        data.totalAdmin = userRoleAccountDb.Items.Count();
                        userRoleAccountDb.Items.ForEach(o => accountIds.Add(o.UserId));

                        userRoleAccountDb = await _userRoleRepository.GetAllDataByExpression(u => u.RoleId == staffId && !accountIds.Contains(u.UserId), 0, 0, null, false, null);
                        data.totalStaff = userRoleAccountDb.Items.Count();
                        userRoleAccountDb.Items.ForEach(o => accountIds.Add(o.UserId));

                        userRoleAccountDb = await _userRoleRepository.GetAllDataByExpression(u => u.RoleId == customerId && !accountIds.Contains(u.UserId), 0, 0, null, false, null);
                        data.totalCustomer = userRoleAccountDb.Items.Count();
                        data.newCustomer = data.totalCustomer - toExcludeOrder.Items.DistinctBy(o => o.AccountId).Count();
                    }
                }
            } catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }


        //// last 7 day, last month, last 3 month, last 6 month, last year
        //// num of order, min max order value, revenue, order value/người, order count/ người
        //public Task<AppActionResult> GetRevenueReport(int time, bool IsForShop);
        //// top 5 Tổng từng số lượng và doanh thu product bán ra, tag nỗi bật 
        //public Task<AppActionResult> GetProductReport(int time, bool IsForShop);
        //// How to minitor new access? Tổng account của sàn, số account mua của shop
        //public Task<AppActionResult> GetUserReport(int time, bool IsForShop);



    }
}