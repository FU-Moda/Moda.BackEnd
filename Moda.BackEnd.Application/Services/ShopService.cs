using AutoMapper;
using Moda.BackEnd.Application.IRepositories;
using Moda.Backend.Domain.Models;
using Moda.BackEnd.Application.IServices;
using Moda.BackEnd.Common.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moda.BackEnd.Common.DTO.Request;
using Moda.BackEnd.Domain.Models;
using Moda.BackEnd.Common.Utils;

namespace Moda.BackEnd.Application.Services
{
    public class ShopService : GenericBackendService, IShopService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Shop> _repository;
        private readonly IUnitOfWork _unitOfWork;
        public ShopService(
            IServiceProvider serviceProvider, IMapper mapper, IRepository<Shop> repository, IUnitOfWork unitOfWork
            ) : base(serviceProvider)
        {
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AppActionResult> AddShop(CreateShopDto dto)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var existedShop = _repository.GetByExpression(p => p.AccountId == dto.AccountId);
                if (existedShop != null)
                {
                    result = BuildAppActionResultError(result, "Shop này đã tồn tại");
                }
                var shop = _mapper.Map<Shop>(dto);
                shop.Id = Guid.NewGuid();
                await _repository.Insert(shop);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> GetAllShop(int pageNumber, int pageSize)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                result.Result = await _repository.GetAllDataByExpression(null, pageNumber, pageSize, null, false, s => s.Account);
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> GetShopAffiliateByShopId(Guid shopId, int pageNumber, int pageSize)
        {
            var result = new AppActionResult();
            var orderDetailRepository = Resolve<IRepository<OrderDetail>>();
            var affiliateRepository = Resolve<IRepository<Affiliate>>();
            var shopRepository = Resolve<IRepository<Shop>>();
            var affiliateList = new List<Affiliate>();
            try
            {
                var shopDb = await shopRepository!.GetByExpression(p => p.Id == shopId);
                if (shopDb == null)
                {
                    result = BuildAppActionResultError(result, $"Không tìm thấy shop với {shopId}");
                }
                var orderOfShop = await orderDetailRepository!.GetAllDataByExpression(p => p.ProductStock!.Product!.ShopId == shopId, pageNumber, pageSize, null, false, p => p.Order!);
                if (orderOfShop!.Items!.Count > 0 && orderOfShop.Items != null)
                {
                    foreach (var item in orderOfShop.Items)
                    {
                        var affiliateOfShop = await affiliateRepository!.GetByExpression(p => p!.OrderId == item.OrderId);
                        if (affiliateOfShop != null)
                        {
                            affiliateList.Add(affiliateOfShop);
                        }
                    }
                    result.Result = new PagedResult<Affiliate>
                    {
                        Items = affiliateList,
                        TotalPages = orderOfShop.TotalPages,
                    };
                }

            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> GetShopByAccountId(string Id)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var accountRepository = Resolve<IRepository<Account>>();
                var accountDb = await accountRepository!.GetByExpression(p => p.Id == Id);
                if (accountDb == null)
                {
                    result = BuildAppActionResultError(result, "Tài khoản này không tồn tại");
                }
                result.Result = await _repository.GetAllDataByExpression(p => p.AccountId == Id, 0, 0, null, false, null);
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> GetShopById(Guid Id)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                ShopDto data = new ShopDto();

                var shopDb = await _repository.GetById(Id);
                if (shopDb == null)
                {
                    result = BuildAppActionResultError(result, $"Không tìm thấy shop với id {Id}");
                    return result;
                }
                data.Shop = shopDb;
                var productRepository = Resolve<IRepository<Product>>();
                var staticFileRepository = Resolve<IRepository<StaticFile>>();
                var productDb = await productRepository!.GetAllDataByExpression(p => p.ShopId == Id, 0, 0, null, false, null);
                if (productDb.Items != null && productDb.Items.Count > 0)
                {
                    foreach (var product in productDb.Items)
                    {
                        var currentStaticFile = await staticFileRepository!.GetAllDataByExpression(s => s.ProductId == product.Id, 0, 0, null, false, null);
                        List<string> imgs = currentStaticFile.Items!.Select(s => s.Img).ToList();
                        data.productResponseDtos!.Add(
                            new ProductResponseDto
                            {
                                Product = product,
                                Img = imgs
                            });
                    }
                    result.Result = data;
                }

            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> GetShopWithBanner(int pageNumber, int pageSize)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var shopPackageRepository = Resolve<IRepository<ShopPackage>>();
                var productRepository = Resolve<IRepository<Product>>();
                var staticFileRepository = Resolve<IRepository<StaticFile>>();
                var shopWithBanner = await shopPackageRepository!.GetAllDataByExpression(s => s.OptionPackageHistory.OptionPackage.IsBannerAvailable,0,0, null, false, s => s.Shop);
                if(shopWithBanner.Items != null && shopWithBanner.Items.Count > 0) {
                    var shopDb = shopWithBanner.Items.Select(s => s.Shop).ToList(); 
                    List < ShopDto > data = new List<ShopDto>();
                    foreach(var shop in shopDb)
                    {
                        var productDb = await productRepository!.GetAllDataByExpression(p => p.ShopId == shop.Id, 0, 0, null, false, null);
                        List<ProductResponseDto> productResponse = new List<ProductResponseDto>();
                        foreach(var product in productDb.Items!)
                        {
                            var img = await staticFileRepository!.GetAllDataByExpression(s => s.ProductId == product.Id, 0, 0, null, false, null);
                            productResponse.Add(
                                new ProductResponseDto
                                {
                                    Product = product,
                                    Img = img.Items!.Select(s => s.Img).ToList()
                                }
                                );
                        }
                        data.Add(new ShopDto
                        {
                            Shop = shop,
                            productResponseDtos = productResponse
                        });
                    }
                    result.Result = new PagedResult<ShopDto>
                    {
                        Items = data.Skip(pageNumber - 1).Take(pageSize).ToList(),
                        TotalPages = data.Count() / pageSize
                    };
                }
            } catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;

        }

        public async Task<AppActionResult> UpdateShop(UpdateShopDto dto)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var shopDb = await _repository.GetById(dto.Id);
                if (shopDb == null)
                {
                    result = BuildAppActionResultError(result, $"Không tìm thấy shop với id {dto.Id}");
                    return result;
                }

                shopDb.Address = dto.Address;
                shopDb.Name = dto.Name;
                shopDb.Description = dto.Description;
                await _repository.Update(shopDb);
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
