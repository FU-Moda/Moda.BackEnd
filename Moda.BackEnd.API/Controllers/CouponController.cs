using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moda.BackEnd.Application.IServices;
using Moda.BackEnd.Common.DTO.Request;
using Moda.BackEnd.Common.DTO.Response;

namespace Moda.BackEnd.API.Controllers
{
    [Route("coupon")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private ICouponService _couponService;
        public CouponController(ICouponService couponService)
        {
            _couponService = couponService; 
        }

        [HttpGet("get-all-coupon/{pageNumber}/{pageSize}")]
        public async Task<AppActionResult> GetAllCoupon(int pageNumber = 1, int pageSize = 10)
        {
            return await _couponService.GetAllCoupon(pageNumber, pageSize);
        }

        [HttpGet("get-coupon-by-id/{couponId}")]
        public async Task<AppActionResult> GetCouponById(Guid couponId)
        {
            return await _couponService.GetCouponById(couponId);    
        }

        [HttpPost("create-coupon")]
        public async Task<AppActionResult> CreateCoupon(CouponDto couponDto)
        {
            return await _couponService.CreateCoupon(couponDto);
        }

        [HttpDelete("delete-coupon")]
        public async Task<AppActionResult> DeleteCoupon(Guid couponId)
        {
            return await _couponService.DeleteCoupon(couponId);
        }

        [HttpPut("update-coupon")]
        public async Task<AppActionResult> UpdateCoupon(CouponDto couponDto)
        {
            return await _couponService.UpdateCoupon(couponDto);    
        }
    }
}
