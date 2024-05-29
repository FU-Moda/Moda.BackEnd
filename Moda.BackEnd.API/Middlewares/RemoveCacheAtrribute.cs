﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Moda.BackEnd.Application.IServices;
using Moda.BackEnd.Common.ConfigurationModel;

namespace Moda.BackEnd.API.Middlewares
{
    public class RemoveCacheAtrribute : Attribute, IAsyncActionFilter
    {

        private readonly string pathEndPoint;

        public RemoveCacheAtrribute(string pathEndPoint)
        {
            this.pathEndPoint = $"/{pathEndPoint}/";
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheConfiguration = context.HttpContext.RequestServices.GetRequiredService<RedisConfiguration>();
            if (!cacheConfiguration.Enabled)
            {
                await next();
                return;
            }
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
            var result = await next();
            if (result.Result is OkResult okObjectResult)
            {
                await cacheService.RemoveCacheResponseAsync(pathEndPoint);
            }
        }
    }
}
