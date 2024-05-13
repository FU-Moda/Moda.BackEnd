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
                    var upload = await filebaseService!.UploadFileToFirebase(productDto.File.Img, pathName);
                    await staticFileRepository!.Insert(new StaticFile
                    {
                        ProductId = productMapper.Id,
                        Img = upload!.Result!.ToString()!,
                    });
                    await _unitOfWork.SaveChangesAsync();
                  
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
            var staticFileRepository = Resolve<IRepository<StaticFile>>();
            var productStockRepository = Resolve<IRepository<ProductStock>>();
            var productResponseList = new List<ProductResponse>();      
            try
            {
                var productList = await _productRepository.GetAllDataByExpression(null, pageNumber, pageSize, null , false, p => p.Shop!, p => p.Shop!);
                if (productList!.Items!.Count > 0 && productList.Items != null)
                {
                    foreach (var item in productList.Items)
                    {
                        var productResponse = new ProductResponse();
                        var productStockDb = await productStockRepository!.GetAllDataByExpression(p => p.ProductId == item.Id, 0, 0, null, false, p => p.Warehouse!);
                        var staticFileDb = await staticFileRepository!.GetAllDataByExpression(p => p.ProductId == item.Id, 0, 0, null, false, p => p.Rating!);
                        if (productStockDb!.Items!.Count > 0 && productStockDb.Items != null && staticFileDb!.Items!.Count > 0 && staticFileDb.Items != null)
                        {
                            productResponse.Product = item;
                            productResponse.StaticFile = staticFileDb!.Items.FirstOrDefault()!;
                            productResponse.ProductStock = productStockDb!.Items.FirstOrDefault()!;
                            productResponseList.Add(productResponse);
                        }
                    }
                    result.Result = new PagedResult<ProductResponse>
                    {
                        Items = productResponseList,
                        TotalPages = productList.TotalPages
                    };
                }
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
                var staticFileRepository = Resolve<IRepository<StaticFile>>();
                var productStockRepository = Resolve<IRepository<ProductStock>>();
                var productResponseList = new List<ProductResponse>();
                var productList = await _productRepository.GetAllDataByExpression(p => p.ClothType == productFilter.ClothType || p.Gender == productFilter.Gender, pageNumber, pageSize, null, false, p => p.Shop!);
                if (productList!.Items!.Count > 0 && productList.Items != null)
                {
                    foreach (var item in productList.Items)
                    {
                        var productResponse = new ProductResponse();
                        var productStockDb = await productStockRepository!.GetAllDataByExpression(p => p.ProductId == item.Id, 0, 0, null, false, p => p.Warehouse!);
                        var staticFileDb = await staticFileRepository!.GetAllDataByExpression(p => p.ProductId == item.Id, 0, 0, null, false, p => p.Rating!);
                        if (productStockDb!.Items!.Count > 0 && productStockDb.Items != null && staticFileDb!.Items!.Count > 0 && staticFileDb.Items != null)
                        {
                            productResponse.Product = item;
                            productResponse.StaticFile = staticFileDb!.Items.FirstOrDefault()!;
                            productResponse.ProductStock = productStockDb!.Items.FirstOrDefault()!;
                            productResponseList.Add(productResponse);
                        }
                    }
                    result.Result = new PagedResult<ProductResponse>
                    {
                        Items = productResponseList,
                        TotalPages = productList.TotalPages,    
                    };
                }
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
                var productDb = await _productRepository.GetByExpression(p => p.Id == productId, p => p.Shop!);
                var staticFileRepository = Resolve<IRepository<StaticFile>>();
                var productStockRepository = Resolve<IRepository<ProductStock>>();
                var productResponse = new ProductResponse();
                if (productDb == null)
                {
                    result = BuildAppActionResultError(result, "Loại sản phẩm này không tồn tại");
                }
                var productStockDb = await productStockRepository!.GetAllDataByExpression(p => p.ProductId == productId, 0, 0, null, false, p => p.Warehouse!);
                var staticFileDb = await staticFileRepository!.GetAllDataByExpression(p => p.ProductId == productId, 0, 0, null, false, p => p.Rating!);
                if (productStockDb!.Items!.Count > 0 && productStockDb.Items != null && staticFileDb!.Items!.Count > 0 && staticFileDb.Items != null)
                {
                    productResponse.Product = productDb!;
                    productResponse.StaticFile = staticFileDb!.Items.FirstOrDefault()!;
                    productResponse.ProductStock = productStockDb!.Items.FirstOrDefault()!;
                }
                result.Result = productResponse;    
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
            var staticFileRepository = Resolve<IRepository<StaticFile>>();
            var productStockRepository = Resolve<IRepository<ProductStock>>();
            var productResponseList = new List<ProductResponse>();
            try
            {
                var productList = await _productRepository.GetAllDataByExpression(p => p.ShopId == shopId, pageNumber, pageSize, null, false, p => p.Shop!);
                if (productList!.Items!.Count > 0 && productList.Items != null)
                {
                    foreach (var item in productList.Items)
                    {
                        var productResponse = new ProductResponse();
                        var productStockDb = await productStockRepository!.GetAllDataByExpression(p => p.ProductId == item.Id, 0, 0, null, false, p => p.Warehouse!);
                        var staticFileDb = await staticFileRepository!.GetAllDataByExpression(p => p.ProductId == item.Id, 0, 0, null, false, p => p.Rating!);
                        if (productStockDb!.Items!.Count > 0 && productStockDb.Items != null && staticFileDb!.Items!.Count > 0 && staticFileDb.Items != null)
                        {
                            productResponse.Product = item;
                            productResponse.StaticFile = staticFileDb!.Items.FirstOrDefault()!;
                            productResponse.ProductStock = productStockDb!.Items.FirstOrDefault()!;
                            productResponseList.Add(productResponse);
                        }
                    }
                    result.Result = new PagedResult<ProductResponse>
                    {
                        Items = productResponseList,
                        TotalPages = productList.TotalPages
                    };
                }
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, $"Có lỗi xảy ra {ex.Message}");
            }
            return result;
        }

        public async Task<AppActionResult> GetProductRatingByProductId(Guid productId, int pageNumber, int pageSize)
        {
            var result = new AppActionResult();
            try
            {
                var productRatingRepository = Resolve<IRepository<Rating>>();
                var staticFileRepository = Resolve<IRepository<StaticFile>>();
                var ratingResponseList = new List<RatingResponse>();
                var productDb = await _productRepository.GetById(productId);
                if (productDb == null)
                {
                    result = BuildAppActionResultError(result, "Loại sản phẩm này không tồn tại");
                }
                var ratingDb = await productRatingRepository!.GetAllDataByExpression(p => p.ProductId == productId, pageNumber, pageSize, null, false,p => p.CreateByAccount!);
                if (ratingDb!.Items!.Count > 0 && ratingDb.Items != null)
                {
                    foreach (var item in ratingDb.Items)
                    {
                        RatingResponse ratingResponse = new RatingResponse();   
                        var ratingImage = await staticFileRepository!.GetByExpression(p => p!.ProductId == item.Id);
                        ratingResponse.Rating = item;
                        ratingResponse.Image = ratingImage!.Img;
                        ratingResponseList.Add(ratingResponse); 
                    }
                }
                result.Result = new PagedResult<RatingResponse>
                {
                    Items = ratingResponseList,
                    TotalPages = ratingDb.TotalPages,
                };
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

                var productDb = await _productRepository.GetByExpression(p => p!.Name.Equals(productDto.Name) && p.ShopId == productDto.ShopId);
                if (productDb == null)
                {
                    result = BuildAppActionResultError(result, "Loại sản phẩm này không tồn tại");
                }

                var oldFile = await staticFileRepository!.GetByExpression(p => p!.ProductId == productDb!.Id);
                if (oldFile == null)
                {
                    result = BuildAppActionResultError(result, "File này không tồn tại");
                }

                var pathName = SD.FirebasePathName.PRODUCT_PREFIX + $"{productDb.Id}.jpg";
                var imageResult = firebaseService!.DeleteFileFromFirebase(pathName);
                if (imageResult != null)
                {
                    result.Messages.Add("Delete image on firebase cloud successful");
                }


                var upload = await firebaseService.UploadFileToFirebase(productDto!.File!.Img, pathName);
                if (upload.IsSuccess && upload.Result != null)
                {
                    result.Messages.Add("Upload image on firebase cloud successful");
                    oldFile.Img = upload.Result.ToString()!;
                }

                _mapper.Map(productDto, productDb);
                await _productRepository.Update(productDb);
                await staticFileRepository.Update(oldFile);
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
