using AutoMapper;
using Moda.Backend.Domain.Models;
using Moda.BackEnd.Application.IRepositories;
using Moda.BackEnd.Application.IServices;
using Moda.BackEnd.Common.DTO.Request;
using Moda.BackEnd.Common.DTO.Response;
using Moda.BackEnd.Common.Utils;
using Moda.BackEnd.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Application.Services
{
    public class ProductService : GenericBackendService, IProductService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Product> _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ProductService(
            IServiceProvider serviceProvider, IMapper mapper, IRepository<Product> productRepository, IUnitOfWork unitOfWork
            ) : base(serviceProvider)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AppActionResult> AddNewProduct(ProductDto productDto)
        {
            var result = new AppActionResult();
            try
            {
                var filebaseService = Resolve<IFirebaseService>();
                var staticFileRepository = Resolve<IRepository<StaticFile>>();
                var productStocksRepository = Resolve<IRepository<ProductStock>>();

                var productIsExist = await _productRepository.GetByExpression(p => p!.Name == productDto.Name);
                if (productIsExist != null)
                {
                    result = BuildAppActionResultError(result, "Loại sản phẩm này đã tồn tại");
                }
                else
                {
                    var productMapper = _mapper.Map<Product>(productDto);
                    productMapper.Id = Guid.NewGuid();
                    await _productRepository.Insert(productMapper);
                    await _unitOfWork.SaveChangesAsync();

                    var productStockList = new List<ProductStock>();
                    foreach (var item in productDto!.ProductStocks!)
                    {
                        productStockList.Add(new ProductStock
                        {
                            Id = Guid.NewGuid(),
                            ProductId = productMapper.Id,
                            Quantity = item.Quantity,
                            ClothingSize = item.ClothingSize,
                            ShoeSize = item.ShoeSize,
                            WarehouseId = item.WarehouseId, 
                        });
                    }

                    await productStocksRepository!.InsertRange(productStockList);
                    await _unitOfWork.SaveChangesAsync();

                    var pathName = SD.FirebasePathName.PRODUCT_PREFIX + $"{productMapper.Id}.jpg";
                    await staticFileRepository!.Insert(new StaticFile
                    {
                        ProductId = productMapper.Id,
                        Img = pathName,
                    });
                    await _unitOfWork.SaveChangesAsync();
                    var upload = await filebaseService!.UploadImageToFirebase(productDto.File.Img, pathName);
                    if (upload.IsSuccess && upload.Result != null)
                        result.Messages.Add("Upload firebase successful");
                    result.Messages.Add(SD.ResponseMessage.CREATE_SUCCESSFULLY);
                }
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, $"Có lỗi xảy ra {ex.Message}");
            }
            return result;
        }

        public async Task<AppActionResult> DeleteProduct(Guid productId)
        {
            var result = new AppActionResult();
            try
            {
                var productDb = _productRepository.GetById(productId);
                if (productDb == null)
                {
                    result = BuildAppActionResultError(result, "Loại sản phẩm này không tồn tại");
                }
                await _productRepository.DeleteById(productId);
                await _unitOfWork.SaveChangesAsync();       
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, $"Có lỗi xảy ra {ex.Message}");
            }
            return result;
        }

        public async Task<AppActionResult> GetAllProduct(int pageNumber, int pageSize)
        {
            var result = new AppActionResult();
            try
            {
                result.Result = await _productRepository.GetAllDataByExpression(null, pageNumber, pageSize, null, false , null);
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, $"Có lỗi xảy ra {ex.Message}");
            }
            return result;
        }

        public async Task<AppActionResult> GetProductByFilter(ProductFilter productFilter, int pageNumber, int pageSize)
        {
            var result = new AppActionResult();
            try
            {
                result.Result = await _productRepository.GetAllDataByExpression(p => p.ClothType == productFilter.ClothType || p.Gender == productFilter.Gender, pageNumber, pageSize, null, false, null);
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, $"Có lỗi xảy ra {ex.Message}");
            }
            return result;
        }

        public async Task<AppActionResult> GetProductById(Guid productId)
        {
            var result = new AppActionResult(); 
            try
            {
                result.Result = await _productRepository.GetById(productId);
            } 
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, $"Có lỗi xảy ra {ex.Message}");
            }
            return result;
        }

        public async Task<AppActionResult> GetProductByShopId(Guid shopId, int pageNumber, int pageSize)
        {
            var result = new AppActionResult();
            try
            {
                result.Result = await _productRepository.GetByExpression(p => p!.ShopId == shopId, p => p.Shop!);
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, $"Có lỗi xảy ra {ex.Message}");
            }
            return result;
        }

        public async Task<AppActionResult> GetProductStockByProductId(Guid productId)
        {
            var result = new AppActionResult();
            try
            {
                var productStockRepository = Resolve<IRepository<ProductStock>>();
                result.Result = await productStockRepository!.GetByExpression(p => p!.ProductId == productId, p => p.Product!);
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, $"Có lỗi xảy ra {ex.Message}");
            }
            return result;
        }

        public async Task<AppActionResult> UpdateProduct(ProductDto productDto)
        {
            var result = new AppActionResult();
            try
            {
                var firebaseService = Resolve<IFirebaseService>();
                var staticFileRepository = Resolve<IRepository<StaticFile>>();
                var productDetailRepository = Resolve<IRepository<ProductStock>>();

                var productDb = await _productRepository.GetByExpression(p => p.Name.Equals(productDto.Name) && p.ShopId == productDto.ShopId);
                if (productDb == null)
                {
                    result = BuildAppActionResultError(result, "Loại sản phẩm này không tồn tại");
                }

                var oldFile = await staticFileRepository!.GetByExpression(p => p!.ProductId == productDb!.Id);
                if (oldFile == null)
                {
                    result = BuildAppActionResultError(result, "File này không tồn tại");
                }

                var imageResult = firebaseService!.DeleteImageFromFirebase(oldFile!.Img);
                if (imageResult != null)
                {
                    result.Messages.Add("Delete image on firebase cloud successful");
                }

                var path = await staticFileRepository.GetById(productDb!.Id);

                var upload = await firebaseService.UploadImageToFirebase(productDto!.File!.Img, path.Img);
                await _productRepository.Update(productDb);
                await _unitOfWork.SaveChangesAsync();
            } 
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, $"Có lỗi xảy ra {ex.Message}");
            }
            return result;
        }
    }
}
