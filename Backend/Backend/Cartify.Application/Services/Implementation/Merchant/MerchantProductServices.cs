using Cartify.Application.Contracts;
using Cartify.Application.Contracts.AttributeDtos;
using Cartify.Application.Contracts.ProductDtos;
using Cartify.Application.Contracts.PromotionDtos;
using Cartify.Application.Services.Implementation.Helper;
using Cartify.Application.Services.Interfaces.Merchant;
using Cartify.Domain.Interfaces.Repositories;
using Cartify.Domain.Models;
using Cartify.Infrastructure.Implementation.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Cartify.Application.Services.Implementation.Merchant
{
    public class MerchantProductServices : IMerchantProductServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileStorageService _fileStorageService;
        private readonly GetUserServices _getUserServices;

        public MerchantProductServices(
            IUnitOfWork unitOfWork,
            IFileStorageService fileStorageService,
            GetUserServices getUserServices)
        {
            _unitOfWork = unitOfWork;
            _fileStorageService = fileStorageService;
            _getUserServices = getUserServices;
        }

        // =========================================================
        // 🔹 PRODUCT CRUD OPERATIONS
        // =========================================================

        #region Product

        public async Task<bool> AddProductAsync(CreateProductDto dto)
        {
            var merchantId = _getUserServices.GetMerchantIdFromToken();
            if (string.IsNullOrEmpty(merchantId))
                return false;

            // استرجع الـ Store بناءً على الـ merchantId
            var store = await _unitOfWork.UserStorerepository.Search(
                s => s.MerchantId == merchantId && !s.IsDeleted);

            if (store == null)
                return false;

            var product = new TblProduct
            {
                ProductName = dto.ProductName,
                ProductDescription = dto.ProductDescription,
                TypeId = dto.TypeId,
                UserStoreId = store.UserStoreId
            };

            await _unitOfWork.ProductRepository.CreateAsync(product);
            return await _unitOfWork.SaveChanges() > 0;
        }


        public async Task<bool> UpdateProductAsync(int productId, UpdateProductDto dto)
        {
            var product = await _unitOfWork.ProductRepository.ReadByIdAsync(productId);
            if (product == null || product.IsDeleted)
                return false;

            if (!string.IsNullOrWhiteSpace(dto.ProductName))
                product.ProductName = dto.ProductName;

            if (!string.IsNullOrWhiteSpace(dto.ProductDescription))
                product.ProductDescription = dto.ProductDescription;

            if (dto.TypeId.HasValue)
                product.TypeId = dto.TypeId.Value;

            // 🔹 Update product images
            if (dto.NewImages != null && dto.NewImages.Any())
            {
                if (product.TblProductImages != null)
                    foreach (var old in product.TblProductImages)
                        old.IsDeleted = true;

                product.TblProductImages = new List<TblProductImage>();

                var username = _getUserServices.GetUserNameFromToken() ?? "product";

                foreach (var image in dto.NewImages)
                {
                    var imageUrl = await _fileStorageService.UploadFileAsync(image, username);
                    product.TblProductImages.Add(new TblProductImage
                    {
                        ProductId = productId,
                        ImageURL = imageUrl,
                        CreatedBy = dto.UpdatedBy ?? 1,
                        IsDeleted = false
                    });
                }
            }

            _unitOfWork.ProductRepository.Update(product);
            return await _unitOfWork.SaveChanges() > 0;
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            var product = await _unitOfWork.ProductRepository.ReadByIdAsync(productId);
            if (product == null)
                return false;

            // Soft delete details and images
            product.IsDeleted = true;

            if (product.TblProductDetails != null)
                foreach (var detail in product.TblProductDetails)
                    detail.IsDeleted = true;

            if (product.TblProductImages != null)
                foreach (var image in product.TblProductImages)
                    image.IsDeleted = true;

            return await _unitOfWork.SaveChanges() > 0;
        }

        #endregion

        // =========================================================
        // 🔹 PRODUCT IMAGES
        // =========================================================

        #region Product Images

        public async Task<bool> AddProductImagesAsync(int productId, List<IFormFile> images)
        {
            var product = await _unitOfWork.ProductRepository.ReadByIdAsync(productId);
            if (product == null || images == null || !images.Any())
                return false;

            product.TblProductImages ??= new List<TblProductImage>();
            var username = _getUserServices.GetUserNameFromToken() ?? "product";
            var merchantId = int.TryParse(_getUserServices.GetMerchantIdFromToken(), out var id) ? id : 0;

            foreach (var image in images)
            {
                var imageUrl = await _fileStorageService.UploadFileAsync(image, username);
                var productImageEntity = new TblProductImage
                {
                    ProductId = productId,
                    ImageURL = imageUrl,
                    CreatedBy = merchantId,
                    IsDeleted = false
                };
                product.TblProductImages.Add(productImageEntity);
            }

            await _unitOfWork.SaveChanges();
            return true;
        }

        #endregion

        // =========================================================
        // 🔹 PRODUCT RETRIEVAL / FILTERING
        // =========================================================

        #region Product Retrieval

        public async Task<ProductDto?> GetProductByIdAsync(int productId)
        {
            var product = await _unitOfWork.ProductRepository.Search(p => p.ProductId == productId);
            if (product == null)
                return null;

            return new ProductDto
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                TypeName = product.Type?.TypeName ?? "N/A",
                CategoryName = product.Type?.Category?.CategoryName ?? "N/A",
                StoreId = product.UserStoreId,
                ImageUrl = product.TblProductImages?.FirstOrDefault()?.ImageURL
            };
        }

        public async Task<PagedResult<ProductDto>> GetAllProductsByMerchantIdAsync(
            string merchantId, int page = 1, int pageSize = 10)
        {
            var store = await _unitOfWork.UserStorerepository.Search(
                s => s.MerchantId == merchantId && !s.IsDeleted);

            if (store == null)
                return new PagedResult<ProductDto>(Enumerable.Empty<ProductDto>(), 0, page, pageSize);

            var allProducts = await _unitOfWork.ProductRepository.GetAllIncluding(
                p => p.Type, p => p.Type.Category, p => p.TblProductImages);

            var productsQuery = allProducts
                .Where(p => p.UserStoreId == store.UserStoreId && !p.IsDeleted);

            var totalCount = productsQuery.Count();

            var pagedProducts = productsQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var productDtos = pagedProducts.Select(p => new ProductDto
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                ProductDescription = p.ProductDescription,
                TypeName = p.Type?.TypeName ?? "",
                CategoryName = p.Type?.Category?.CategoryName ?? "",
                StoreId = p.UserStoreId,
                ImageUrl = p.TblProductImages?.FirstOrDefault()?.ImageURL
            }).ToList();

            return new PagedResult<ProductDto>(productDtos, totalCount, page, pageSize);
        }

        public async Task<PagedResult<ProductDto>> GetProductsByNameAsync(
            string name, int page = 1, int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(name))
                return new PagedResult<ProductDto>(Enumerable.Empty<ProductDto>(), 0, page, pageSize);

            var allProducts = await _unitOfWork.ProductRepository.GetAllIncluding(
                p => p.Type, p => p.Type.Category, p => p.TblProductImages);

            var filteredProducts = allProducts
                .Where(p => p.ProductName.Contains(name) && !p.IsDeleted);

            var totalCount = filteredProducts.Count();

            var pagedProducts = filteredProducts
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();


            var productDtos = pagedProducts.Select(p => new ProductDto
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                ProductDescription = p.ProductDescription,
                TypeName = p.Type?.TypeName ?? "",
                CategoryName = p.Type?.Category?.CategoryName ?? "",
                StoreId = p.UserStoreId,
                ImageUrl = p.TblProductImages?.FirstOrDefault()?.ImageURL
            }).ToList();

            return new PagedResult<ProductDto>(productDtos, totalCount, page, pageSize);
        }

        public async Task<PagedResult<ProductDto>> GetProductsByTypeIdAsync(
            int typeId, int page = 1, int pageSize = 10)
        {
            if (typeId <= 0)
                return new PagedResult<ProductDto>(Enumerable.Empty<ProductDto>(), 0, page, pageSize);

            var allProducts = await _unitOfWork.ProductRepository.GetAllIncluding(
                p => p.Type, p => p.Type.Category, p => p.TblProductImages);

            var filteredProducts = allProducts
                .Where(p => p.TypeId == typeId && !p.IsDeleted);

            var totalCount = filteredProducts.Count();

            var pagedProducts = filteredProducts
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var productDtos = pagedProducts.Select(p => new ProductDto
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                ProductDescription = p.ProductDescription,
                TypeName = p.Type?.TypeName ?? "",
                CategoryName = p.Type?.Category?.CategoryName ?? "",
                StoreId = p.UserStoreId,
                ImageUrl = p.TblProductImages?.FirstOrDefault()?.ImageURL
            }).ToList();

            return new PagedResult<ProductDto>(productDtos, totalCount, page, pageSize);
        }

        #endregion

        // =========================================================
        // 🔹 PRODUCT DETAILS
        // =========================================================

        #region Product Details

        public async Task<ProductDetailDto?> GetProductDetailAsync(int productDetailId)
        {
            var query = _unitOfWork.ProductDetailsRepository.GetAllIncluding2(
                p => p.Product,
                p => p.Product.TblProductImages,
                p => p.Inventory,
                p => p.LkpProductDetailsAttributes,
                p => p.LkpProductDetailsAttributes.Select(a => a.Attribute),
                p => p.LkpProductDetailsAttributes.Select(a => a.MeasureUnit)
            );

            var productDetail = await query
                .FirstOrDefaultAsync(p => p.ProductDetailId == productDetailId && !p.IsDeleted);

            if (productDetail == null)
                return null;

            var dto = new ProductDetailDto
            {
                ProductDetailId = productDetail.ProductDetailId,
                Description = productDetail.Description,
                Price = productDetail.Price,
                Serial = productDetail.SerialNumber,
                QuantityAvailable = productDetail.Inventory?.QuantityAvailable ?? 0,
                Images = productDetail.Product?.TblProductImages?
                    .Select(i => i.ImageURL).ToList() ?? new List<string>(),
                Attributes = productDetail.LkpProductDetailsAttributes?
                    .Select(a => new AttributeDto
                    {
                        Name = a.Attribute?.Name,
                        MeasureUnit = a.MeasureUnit?.Name
                    }).ToList() ?? new List<AttributeDto>()
                   
            };

            if (dto.Promotions.Any())
            {
                var totalDiscount = dto.Promotions.Sum(p => p.DiscountPercentage);
                dto.PriceAfterDiscount = dto.Price - (dto.Price * totalDiscount / 100);
            }

            return dto;
        }

        public async Task<bool> AddProductDetailAsync(CreateProductDetailDto dto)
        {
            var serial = await GenerateGlobalSerialAsync();
            var productDetail = new TblProductDetail
            {
                SerialNumber = serial,
                ProductId = dto.ProductId,
                Price = dto.Price,
                Description = dto.Description,
                CreatedDate = DateTime.Now,
                IsDeleted = false
            };

            var inventory = new TblInventory
            {
                InventoryId = Guid.NewGuid().GetHashCode(),
                ProductDetail = productDetail,
                QuantityAvailable = dto.QuantityAvailable,
                CreatedDate = DateTime.Now
            };

            productDetail.LkpProductDetailsAttributes = dto.Attributes.Select(a => new LkpProductDetailsAttribute
            {
                AttributeId = a.AttributeId,
                MeasureUnitId = a.MeasureUnitId
            }).ToList();

            await _unitOfWork.ProductDetailsRepository.CreateAsync(productDetail);
            await _unitOfWork.InventoryRepository.CreateAsync(inventory);

            return await _unitOfWork.SaveChanges() > 0;
        }

        public async Task<bool> UpdateProductDetailAsync(UpdateProductDetailDto dto)
        {
            var productDetail = await _unitOfWork.ProductDetailsRepository
                .ReadByIdAsync(dto.ProductDetailId);

            if (productDetail == null || productDetail.IsDeleted)
                return false;

            productDetail.Price = (decimal)dto.Price;
            productDetail.Description = dto.Description;

            if (productDetail.Inventory != null)
                productDetail.Inventory.QuantityAvailable = (int)dto.QuantityAvailable;

            productDetail.LkpProductDetailsAttributes?.Clear();
            productDetail.LkpProductDetailsAttributes = dto.Attributes.Select(a => new LkpProductDetailsAttribute
            {
                ProductDetailId = dto.ProductDetailId,
                AttributeId = a.AttributeId,
                MeasureUnitId = a.MeasureUnitId
            }).ToList();

            _unitOfWork.ProductDetailsRepository.Update(productDetail);
            return await _unitOfWork.SaveChanges() > 0;
        }

        public async Task<bool> DeleteProductDetailAsync(int productDetailId)
        {
            var productDetail = await _unitOfWork.ProductDetailsRepository.ReadByIdAsync(productDetailId);
            if (productDetail == null || productDetail.IsDeleted)
                return false;

            productDetail.IsDeleted = true;
            productDetail.DeletedDate = DateTime.Now;
            productDetail.DeletedBy = int.TryParse(_getUserServices.GetMerchantIdFromToken(), out var merchantId) ? merchantId : 0;

            _unitOfWork.ProductDetailsRepository.Update(productDetail);
            return await _unitOfWork.SaveChanges() > 0;
        }

        #endregion

        // =========================================================
        // 🔹 PRODUCT DETAILS
        // =========================================================


        public async Task<string> GenerateGlobalSerialAsync()
        {
            var lastDetail = await _unitOfWork.ProductDetailsRepository.GetAllIncluding2()
                .OrderByDescending(p => p.ProductId)
                .FirstOrDefaultAsync();

            long nextNumber = 1;

            if (lastDetail != null)
            {
                var parts = lastDetail.SerialNumber.Split('-'); // ["PD", "2025", "000123"]
                if (parts.Length == 3 && long.TryParse(parts[2], out long lastNum))
                {
                    nextNumber = lastNum + 1;
                }
            }

            string year = DateTime.UtcNow.Year.ToString();
            return $"PD-{year}-{nextNumber:D6}";
        }
    }

 }
