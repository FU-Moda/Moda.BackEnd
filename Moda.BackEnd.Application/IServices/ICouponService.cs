using Moda.BackEnd.Common.DTO.Request;
using Moda.BackEnd.Common.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Application.IServices
{
    public interface ICouponService
    {
        Task<AppActionResult> CreateCoupon(CouponDto couponDto);
        Task<AppActionResult> UpdateCoupon(CouponDto couponDto);
        Task<AppActionResult> DeleteCoupon(Guid couponId);
        Task<AppActionResult> GetAllCoupon(int pageNumber, int pageSize);
        Task<AppActionResult> GetCouponById(Guid couponId);
    }
}
