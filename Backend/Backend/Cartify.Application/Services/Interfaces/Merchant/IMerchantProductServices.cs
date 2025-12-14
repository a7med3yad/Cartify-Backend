using Cartify.Application.Contracts;
using Cartify.Application.Contracts.ProductDtos;
using Cartify.Infrastructure.Implementation.Repository;
using Microsoft.AspNetCore.Http;

namespace Cartify.Application.Services.Interfaces.Merchant
{
    public interface IMerchantProductServices
    {
        Task<PagedResult<ProductDto>> GetAllProductsByMerchantIdAsync(string merchantId, int page = 1, int pageSize = 10);
        Task<PagedResult<ProductDto>> GetProductsByTypeIdAsync(int typeId, int page = 1, int pageSize = 10);
        Task<PagedResult<ProductDto>> GetProductsByNameAsync(string name, int page = 1, int pageSize = 10);
        Task<ProductDto?> GetProductByIdAsync(int productId);
        Task<bool> AddProductAsync(CreateProductDto dto);
        Task<bool> AddProductImagesAsync(int productId, List<IFormFile> images);
        Task<bool> UpdateProductAsync(int productId, UpdateProductDto dto);
        Task<bool> DeleteProductAsync(int productId);
        Task<ProductDetailDto?> GetProductDetailAsync(int productDetailId);
        Task<bool> DeleteProductDetailAsync(int productDetailId);
        Task<bool> UpdateProductDetailAsync(UpdateProductDetailDto dto);
        Task<bool> AddProductDetailAsync(CreateProductDetailDto dto);
    }
}
