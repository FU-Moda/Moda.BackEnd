﻿using Moda.Backend.Domain.Models;
using Moda.BackEnd.Common.DTO.Request;
using Moda.BackEnd.Common.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Application.IServices
{
    public interface IProductService
    {
        Task<AppActionResult> GetAllProduct(int pageNumber, int pageSize);
        Task<AppActionResult> AddNewProduct(ProductDto productDto);
        Task<AppActionResult> UpdateProduct(ProductDto productDto);   
        Task<AppActionResult> DeleteProduct(Guid productId);
        Task<AppActionResult> GetProductById(Guid productId);
        Task<AppActionResult> GetProductByShopId(Guid shopId, int pageNumber, int pageSize);
        Task<AppActionResult> GetProductByFilter(ProductFilter productFilter, int pageNumber, int pageSize);
        Task<AppActionResult> GetProductStockByProductId(Guid productId);
        Task<AppActionResult> GetProductRatingByProductId(Guid productId, int pageNumber, int pageSize);
    }
}
