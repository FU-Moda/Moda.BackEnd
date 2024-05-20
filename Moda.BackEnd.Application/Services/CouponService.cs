using AutoMapper;
using Moda.BackEnd.Application.IRepositories;
using Moda.BackEnd.Application.IServices;
using Moda.BackEnd.Common.DTO.Request;
using Moda.BackEnd.Common.DTO.Response;
using Moda.BackEnd.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Application.Services
{
    public class CouponService : GenericBackendService, ICouponService
    {
        public readonly IRepository<Coupon> _couponRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CouponService(
            IServiceProvider serviceProvider,
            IRepository<Coupon> couponRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork
            ) : base(serviceProvider)
        {
            _couponRepository = couponRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<AppActionResult> CreateCoupon(CouponDto couponDto)
        {
            var result = new AppActionResult();
            try
            {
                var coupon = await _couponRepository.GetByExpression(p => p!.Id == couponDto.Id);
                if (coupon != null)
                {
                    result = BuildAppActionResultError(result, "Voucher này đã tồn tại");
                }
                var couponDb = _mapper.Map<Coupon>(couponDto);
                result.Result = await _couponRepository.Insert(couponDb);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> DeleteCoupon(Guid couponId)
        {
            var result = new AppActionResult();
            try
            {
                var couponDb = _couponRepository.GetByExpression(p => p!.Id == couponId);
                if (couponDb == null)
                {
                    result = BuildAppActionResultError(result, "Voucher này không tồn tại");
                }
                await _couponRepository.DeleteById(couponId);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> GetAllCoupon(int pageNumber, int pageSize)
        {
            var result = new AppActionResult();
            try
            {
                result.Result = await _couponRepository.GetAllDataByExpression(null, pageNumber, pageSize, null, false, null);
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> GetCouponById(Guid couponId)
        {
            var result = new AppActionResult();
            try
            {
                result.Result = await _couponRepository.GetByExpression(p => p!.Id == couponId);
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> UpdateCoupon(CouponDto couponDto)
        {
            var result = new AppActionResult();
            try
            {
                var couponDb = await _couponRepository.GetByExpression(p => p!.Id == couponDto.Id);
                if (couponDb == null)
                {
                    result = BuildAppActionResultError(result, "Voucher này không tồn tại");
                }
                _mapper.Map(couponDto, couponDb);
                await _couponRepository.Update(couponDb);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }
    }
}
