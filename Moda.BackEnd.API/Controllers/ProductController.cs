using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moda.BackEnd.API.Middlewares;
using Moda.BackEnd.Application.IServices;
using Moda.BackEnd.Common.DTO.Request;
using Moda.BackEnd.Common.DTO.Response;
using Moda.BackEnd.Domain.Enum;

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
        [CacheAttribute(259200)]
        public async Task<AppActionResult> GetAllProduct(int pageNumber = 1, int pageSize = 10)
        {
            return await _productService.GetAllProduct(pageNumber, pageSize);
        }

        [HttpGet("get-product-by-id/{productId}")]
        [CacheAttribute(259200)]
        public async Task<AppActionResult> GetProductById(Guid productId)
        {
            return await _productService.GetProductById(productId); 
        }

        [HttpGet("get-product-by-shop-id/{shopId}/{pageNumber}/{pageSize}")]
        [CacheAttribute(259200)]
        public async Task<AppActionResult> GetProductByShopId(Guid shopId, int pageNumber = 1, int pageSize = 10)
        {
            return await _productService.GetProductByShopId(shopId, pageNumber, pageSize);
        }

        [HttpGet("get-product-by-status/{pageNumber}/{pageSize}")]
        [CacheAttribute(259200)]
        public async Task<AppActionResult> GetProductByStatus(ProductStatus productStatus, int pageNumber = 1, int pageSize = 10)
        {
            return await _productService.GetProductByStatus(productStatus, pageNumber, pageSize);
        }

        [HttpPost("get-product-by-filter/{pageNumber}/{pageSize}")]
        [CacheAttribute(259200)]
        public async Task<AppActionResult> GetProductByFilter(ProductFilter productFilter, int pageNumber = 1, int pageSize = 10)
        {
            return await _productService.GetProductByFilter(productFilter, pageNumber, pageSize);       
        }

        [HttpGet("get-product-stock-by-product-id/{productId}")]
        [CacheAttribute(259200)]
        public async Task<AppActionResult> GetProductStockByProductId(Guid productId, int pageNumber = 1, int pageSize = 10)
        {
            return await _productService.GetProductStockByProductId(productId, pageNumber, pageSize);
        }

        [HttpPost("add-new-product")]
        [RemoveCacheAtrribute("product")]
        public async Task<AppActionResult> AddNewProduct([FromForm]ProductDto productDto)
        {
            return await _productService.AddNewProduct(productDto);
        }

        [HttpDelete("delete-product")]
        [RemoveCacheAtrribute("product")]
        public async Task<AppActionResult> DeleteProduct(Guid productId)
        {
            return await _productService.DeleteProduct(productId);  
        }

        [HttpPut("update-product")]
        [RemoveCacheAtrribute("product")]
        public async Task<AppActionResult> UpdateProduct([FromForm]UpdateProductDto productDto)
        {
            return await _productService.UpdateProduct(productDto);     
        }
        [HttpGet("get-rating-by-product-id/{productId}/{pageNumber}/{pageSize}")]
        [CacheAttribute(259200)]
        public async Task<AppActionResult> GetProductRatingByProductId(Guid productId, int pageNumber = 1, int pageSize = 10)
        {
            return await _productService.GetProductRatingByProductId(productId, pageNumber, pageSize);
        }
        [HttpPost("update-product-status/{productId}")]
        public async Task<AppActionResult> UpdateProductStatus(Guid productId, ProductStatus productStatus)
        {
            return await _productService.UpdateProductStatus(productId, productStatus);     
        }

        [RemoveCacheAtrribute("product")]
        [HttpGet("product")]
        public IActionResult RemoveCache()
        {
            return Ok();
        }
    }
}
