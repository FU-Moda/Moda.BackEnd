﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moda.BackEnd.Application.IServices;
using Moda.BackEnd.Common.DTO.Request;
using Moda.BackEnd.Common.DTO.Response;

namespace Moda.BackEnd.API.Controllers
{
    [Route("product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;   
        }

        [HttpGet("get-all-product/{pageNumber}/{pageSize}")]
        public async Task<AppActionResult> GetAllProduct(int pageNumber = 1, int pageSize = 10)
        {
            return await _productService.GetAllProduct(pageNumber, pageSize);
        }

        [HttpGet("get-product-by-id/{productId}")]
        public async Task<AppActionResult> GetProductById(Guid productId)
        {
            return await _productService.GetProductById(productId); 
        }

        [HttpGet("get-product-by-shop-id/{shopId}/{pageNumber}/{pageSize}")]
        public async Task<AppActionResult> GetProductByShopId(Guid shopId, int pageNumber = 1, int pageSize = 10)
        {
            return await _productService.GetProductByShopId(shopId, pageNumber, pageSize);
        }

        [HttpPost("get-product-by-filter/{pageNumber}/{pageSize}")]
        public async Task<AppActionResult> GetProductByFilter(ProductFilter productFilter, int pageNumber = 1, int pageSize = 10)
        {
            return await _productService.GetProductByFilter(productFilter, pageNumber, pageSize);       
        }

        [HttpGet("get-product-stock-by-product-id/{productId}")]
        public async Task<AppActionResult> GetProductStockByProductId(Guid productId)
        {
            return await _productService.GetProductStockByProductId(productId);
        }

        [HttpPost("add-new-product")]
        public async Task<AppActionResult> AddNewProduct([FromForm]ProductDto productDto)
        {
            return await _productService.AddNewProduct(productDto);
        }

        [HttpDelete("delete-product")]
        public async Task<AppActionResult> DeleteProduct(Guid productId)
        {
            return await _productService.DeleteProduct(productId);  
        }

        [HttpPut]
        public async Task<AppActionResult> UpdateProduct([FromForm]ProductDto productDto)
        {
            return await _productService.UpdateProduct(productDto);     
        }
    }
}
