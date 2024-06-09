﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moda.BackEnd.API.Middlewares;
using Moda.BackEnd.Application.IServices;
using Moda.BackEnd.Common.DTO.Response;

namespace Moda.BackEnd.API.Controllers
{
    [Route("dashboard")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private IDashBoardService _service;
        public DashboardController(IDashBoardService service)
        {
            _service = service;
        }

        [HttpGet("get-user-report")]
        [CacheAttribute(259200)]
        public async Task<AppActionResult> GetUserReport(int timePeriod, Guid? shopId)
        {
            return await _service.GetUserReport(timePeriod, shopId);
        }

        [HttpGet("get-product-report")]
        [CacheAttribute(259200)]
        public async Task<AppActionResult> GetProductReport(int timePeriod, Guid? shopId)
        {
            return await _service.GetProductReport(timePeriod, shopId);
        }

        [HttpGet("get-revenue-report")]
        [CacheAttribute(259200)]
        public async Task<AppActionResult> GetRevenueReport(int timePeriod, Guid? shopId)
        {
            return await _service.GetRevenueReport(timePeriod, shopId);
        }
    }
}
