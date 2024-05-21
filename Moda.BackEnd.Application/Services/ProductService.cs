using AutoMapper;
using Moda.Backend.Domain.Models;
using Moda.BackEnd.Application.IRepositories;
using Moda.BackEnd.Application.IServices;
using Moda.BackEnd.Common.DTO.Request;
using Moda.BackEnd.Common.DTO.Response;
using Moda.BackEnd.Common.Utils;
using Moda.BackEnd.Domain.Enum;
using Moda.BackEnd.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

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
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var firebaseService = Resolve<IFirebaseService>();
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
                                Price = item.Price,
                            });
                        }

                        ;
                        List<StaticFile> staticList = new List<StaticFile>();
                        foreach (var file in productDto!.Img)
                        {
                            var pathName = SD.FirebasePathName.PRODUCT_PREFIX + $"{productMapper.Id}{Guid.NewGuid()}.jpg";
                            var upload = await firebaseService!.UploadFileToFirebase(file, pathName);
                            var staticImg = new StaticFile
                            {
                                Id = Guid.NewGuid(),
                                ProductId = productMapper.Id,
                                Img = upload!.Result!.ToString()!,
                            };
                            staticList.Add(staticImg);

                            if (!upload.IsSuccess)
                            {
                                result = BuildAppActionResultError(result, "Upload failed");
                            }
                        }
                        if (!BuildAppActionResultIsError(result))
                        {
                            await _productRepository.Insert(productMapper);
                            await productStocksRepository!.InsertRange(productStockList);
                            await staticFileRepository!.InsertRange(staticList);
                            await _unitOfWork.SaveChangesAsync();
                            scope.Complete();
                        }
                    }
                }
                catch (Exception ex)
                {
                    result = BuildAppActionResultError(result, $"Có lỗi xảy ra {ex.Message}");
                }
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
                var productList = await _productRepository.GetAllDataByExpression(null, pageNumber, pageSize, null, false, p => p.Shop!, p => p.Shop!);
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
                            productResponse.StaticFile = staticFileDb!.Items;
                            productResponse.ProductStock = productStockDb!.Items;
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
                            productResponse.StaticFile = staticFileDb!.Items;
                            productResponse.ProductStock = productStockDb!.Items;
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
                    productResponse.StaticFile = staticFileDb!.Items;
                    productResponse.ProductStock = productStockDb!.Items;
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
                        if (productStockDb!.Items!.Count > 0 && productStockDb.Items != null || staticFileDb!.Items!.Count > 0 && staticFileDb.Items != null)
                        {
                            productResponse.Product = item;
                            productResponse.StaticFile = staticFileDb.Items.ToList();
                            productResponse.ProductStock = productStockDb!.Items!;
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
                var ratingDb = await productRatingRepository!.GetAllDataByExpression(p => p.ProductId == productId, pageNumber, pageSize, null, false, p => p.CreateByAccount!);
                if (ratingDb!.Items!.Count > 0 && ratingDb.Items != null)
                {
                    foreach (var item in ratingDb.Items)
                    {
                        RatingResponse ratingResponse = new RatingResponse();
                        var ratingImage = await staticFileRepository!.GetAllDataByExpression(p => p!.RatingId == item.Id, 0, 0, null, false, null);
                        ratingResponse.Rating = item;
                        ratingResponse.Image = ratingImage!.Items!.Select(p => p.Img).ToList();
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

        public async Task<AppActionResult> GetProductStockByProductId(Guid productId, int pageNumber, int pageSize)
        {
            var result = new AppActionResult();
            try
            {
                var productStockRepository = Resolve<IRepository<ProductStock>>();
                var productStockDb = await productStockRepository!.GetAllDataByExpression(p => p!.ProductId == productId, pageNumber, pageSize, null, false, p => p.Product!.Shop!, p => p.Warehouse!);
                if (productStockDb!.Items!.Count > 0 && productStockDb.Items != null)
                {
                    // Assume the product details are the same for all product stock entries, so take the first one
                    var product = productStockDb.Items.First().Product;

                    var productStockResponse = new ProductStockResponse
                    {
                        Product = product!,
                        ProductStock = productStockDb.Items!
                    };
                    result.Result = productStockResponse;
                }
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, $"Có lỗi xảy ra {ex.Message}");
            }
            return result;
        }

        public async Task<AppActionResult> UpdateProduct(UpdateProductDto productDto)
        {
            var result = new AppActionResult();
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var firebaseService = Resolve<IFirebaseService>();
                    var staticFileRepository = Resolve<IRepository<StaticFile>>();
                    var productDetailRepository = Resolve<IRepository<ProductStock>>();
                    var productDb = await _productRepository.GetByExpression(p => p!.Name.Equals(productDto.Name) && p.ShopId == productDto.ShopId);
                    if (productDb == null)
                    {
                        return BuildAppActionResultError(result, "Loại sản phẩm này không tồn tại");
                    }

                    

                    var oldFiles = await staticFileRepository!.GetAllDataByExpression(p => p!.ProductId == productDb!.Id, 0, 0, null, false, null);
                    if (oldFiles.Items == null || !oldFiles.Items.Any())
                    {
                        return BuildAppActionResultError(result, "File này không tồn tại");
                    }

                    var oldFileList = new List<StaticFile>();
                    foreach (var oldImg in oldFiles.Items!)
                    {
                        oldFileList.Add(oldImg);
                        var pathName = SD.FirebasePathName.PRODUCT_PREFIX + $"{productDb!.Id}.jpg";
                        var imageResult = firebaseService!.DeleteFileFromFirebase(pathName);
                        if (imageResult != null)
                        {
                            result.Messages.Add("Delete image on firebase cloud successful");
                        }
                    }

                    // Debugging: Check if oldFileList has items
                    if (!oldFileList.Any())
                    {
                        return BuildAppActionResultError(result, "No old files found to delete.");
                    }

                    // Attempt to delete old files
                    await staticFileRepository.DeleteRange(oldFileList);
                    await _unitOfWork.SaveChangesAsync();

                    List<StaticFile> staticList = new List<StaticFile>();
                    foreach (var file in productDto!.Img)
                    {
                        var pathName = SD.FirebasePathName.PRODUCT_PREFIX + $"{productDb.Id}{Guid.NewGuid()}.jpg";
                        var upload = await firebaseService!.UploadFileToFirebase(file, pathName);
                        var staticImg = new StaticFile
                        {
                            Id = Guid.NewGuid(),
                            ProductId = productDb.Id,
                            Img = upload!.Result!.ToString()!,
                        };
                        staticList.Add(staticImg);

                        if (!upload.IsSuccess)
                        {
                            return BuildAppActionResultError(result, "Upload failed");
                        }
                    }

                    var productStock = await productDetailRepository!.GetAllDataByExpression(p => p.ProductId == productDto.Id, 0, 0, null, false, null);
                    if (productStock.Items != null && productStock.Items.Count > 0)
                    {
                        foreach (var updateStock in productDto.ProductStocks!)
                        {
                            var stockItem = productStock.Items.FirstOrDefault(s => s.ClothingSize == updateStock.ClothingSize || s.ShoeSize == updateStock.ShoeSize);
                            if (stockItem != null)
                            {
                                stockItem.ClothingSize = updateStock.ClothingSize;
                                stockItem.ShoeSize = updateStock.ShoeSize;
                                stockItem.Quantity = updateStock.Quantity;
                                stockItem.Price = updateStock.Price;
                                await productDetailRepository.Update(stockItem);
                            }
                        }
                    }

                    if (!BuildAppActionResultIsError(result))
                    {
                        _mapper.Map(productDto, productDb);
                        await _productRepository.Update(productDb!);
                        await staticFileRepository.InsertRange(staticList);
                        await _unitOfWork.SaveChangesAsync();
                        result.Messages.Add("Update sản phẩm thành công");
                        scope.Complete();
                    }
                }
                catch (Exception ex)
                {
                    return BuildAppActionResultError(result, $"Có lỗi xảy ra {ex.Message}");
                }
            }
            return result;
        }

    }
}
