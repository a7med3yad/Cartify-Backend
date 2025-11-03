using Cartify.Application.Contracts;
using Cartify.Application.Contracts.CategoryDtos;
using Cartify.Application.Contracts.ProductDtos;
using Cartify.Infrastructure.Implementation.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cartify.Application.Services.Interfaces.Merchant
{
    public interface IMerchantCategoryServices
    {
        Task<bool> CreateCategoryAsync(CreateCategoryDto dto);
        Task<bool> UpdateCategoryAsync(int categoryId, CreateCategoryDto dto);
        Task<bool> DeleteCategoryAsync(int categoryId);
        Task<PagedResult<CategoryDto>> GetAllCategoriesAsync(int page = 1, int pageSize = 10);
        Task<CategoryDto?> GetCategoryByIdAsync(int categoryId);
        Task<int> GetProductCountByCategoryIdAsync(int categoryId);
        Task<PagedResult<ProductDto>> GetProductsByCategoryIdAsync(int categoryId, int page = 1, int pageSize = 10);
        Task<PagedResult<ProductDto>> GetProductsBySubCategoryIdAsync(int subCategoryId, int page = 1, int pageSize = 10);
        Task<IEnumerable<SubCategoryDto>> GetAllSubCategoriesAsync();
        Task<SubCategoryDto?> GetSubCategoryByIdAsync(int id);
        Task<bool> CreateSubCategoryAsync(CreateSubCategoryDto dto);
        Task<bool> UpdateSubCategoryAsync(int subCategoryId, CreateSubCategoryDto dto);
        Task<bool> DeleteSubCategoryAsync(int subCategoryId);
    }
}
