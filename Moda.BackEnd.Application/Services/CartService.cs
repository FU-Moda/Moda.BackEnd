using AutoMapper;
using Moda.Backend.Domain.Models;
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
    public class CartService : GenericBackendService, ICartService
    {
        private readonly IRepository<Cart> _cartRepository; 
        private readonly IRepository<CartDetail> _cartDetailRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CartService(
            IServiceProvider serviceProvider, IUnitOfWork unitOfWork,
            IRepository<Cart> cartRepository, IRepository<CartDetail> cartDetailRepository,
            IMapper mapper
            ) : base(serviceProvider)
        {
            _cartRepository = cartRepository;
            _cartDetailRepository = cartDetailRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<AppActionResult> GetCartItems(string accountId)
        {
            var result = new AppActionResult();
            try 
            {
                var cartDb = await _cartRepository.GetByExpression(p => p!.AccountId == accountId, p => p.Account!);
                var cartDetailDb = await _cartDetailRepository.GetAllDataByExpression(p => p.Cart!.AccountId == accountId, 0, 0, null, false, p => p.Product!
                    );
                var cartReponse = new CartResponse();
                if (cartDb == null)
                {
                    result = BuildAppActionResultError(result, "Giỏ hàng của tài khoản này không tồn tại");
                }
                cartReponse.Cart = cartDb!;
                cartReponse.CartDetail = cartDetailDb!.Items!.ToList();
                result.Result = cartReponse;    
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, $"Có lỗi xảy ra {ex.Message}");
            }
            return result;      
        }

        public async Task<AppActionResult> UpdateCartDetail(string accountId, IEnumerable<CartDetailDto> cartDetailDtos, int pageNumber, int pageSize)
        {
            var result = new AppActionResult();
            try
            {
                var productStockRepository = Resolve<IRepository<ProductStock>>();

                var cart = await _cartRepository.GetByExpression(p => p!.AccountId == accountId);
                if (cart == null)
                {
                    var cartForNewUser = new Cart { AccountId = accountId };
                    await _cartRepository.Insert(cartForNewUser);
                    await _unitOfWork.SaveChangesAsync();
                    var cartItemInsert = _mapper.Map<IEnumerable<CartDetail>>(cartDetailDtos);
                    foreach (var item in cartItemInsert)
                    {
                        item.CartId = cartForNewUser.Id;    
                    }
                    await _cartDetailRepository.InsertRange(cartItemInsert);
                    await _unitOfWork.SaveChangesAsync();   
                }
                else
                {
                    var itemsInCart = await _cartDetailRepository.GetAllDataByExpression(p => p.CartId == cart!.Id, 0, 0, null, false, p => p.Product!);
                    if (itemsInCart!.Items!.Count > 0 && itemsInCart.Items != null)
                    {
                        foreach (var item in itemsInCart.Items)
                        {
                            foreach (var cartItem in cartDetailDtos)
                            {
                                if (cartItem.ProductId == item.ProductId)
                                {
                                    var productStockDb = await productStockRepository!.GetByExpression(p => p!.ProductId == cartItem.ProductId, p => p.Product!);
                                    if (productStockDb!.Quantity < cartItem.Count)
                                    {
                                        result = BuildAppActionResultError(result, $"Sản phẩm với mã {cartItem.ProductId} với số lượng {cartItem.Count} không đủ đáp ứng");
                                    }
                                    else
                                    {
                                        if (item.Count - cartItem.Count > 0)
                                        {
                                            item.Count += cartItem.Count;
                                            if (item.Count <= 0)
                                            {
                                                await _cartDetailRepository.DeleteById(item.ProductId);
                                                await _unitOfWork.SaveChangesAsync();
                                            }
                                        }
                                        else if (item.Count - cartItem.Count <= 0)
                                        {
                                            await _cartDetailRepository.DeleteById(item.ProductId);
                                            await _unitOfWork.SaveChangesAsync();
                                        }
                                    }
                                }
                                if (!itemsInCart.Items.Any())
                                {
                                    await _cartDetailRepository.DeleteById(item.CartId);
                                    await _unitOfWork.SaveChangesAsync();
                                }
                                await _unitOfWork.SaveChangesAsync();
                            }
                        }
                    }
                }
                result.Result = _cartDetailRepository.GetAllDataByExpression(p => p.Cart!.AccountId == accountId, pageNumber, pageSize , null, false, p => p.Product!);
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, $"Có lỗi xảy ra {ex.Message}");
            }
            return result; 
        }
    }
}
