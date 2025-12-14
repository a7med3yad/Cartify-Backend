using Cartify.Application.Contracts.InventoryDtos;
using Cartify.Infrastructure.Implementation.Repository;
using System.Threading.Tasks;

namespace Cartify.Application.Services.Interfaces.Merchant
{
    public interface IMerchantInventoryServices
    {
        /// <summary>
        /// Get paged list of inventory items for a merchant store.
        /// </summary>
        Task<PagedResult<InventoryDto>> GetInventoryByStoreIdAsync(
            int storeId,
            int page = 1,
            int pageSize = 10);

        /// <summary>
        /// Get inventory info for a specific product detail (variant).
        /// </summary>
        Task<InventoryDto?> GetInventoryByProductDetailIdAsync(int productDetailId);

        /// <summary>
        /// Update stock quantity of a specific product detail.
        /// </summary>
        Task<bool> UpdateStockQuantityAsync(int productDetailId, int newQuantity);

        /// <summary>
        /// Get all items that reached or are below their reorder level.
        /// </summary>
        Task<PagedResult<InventoryDto>> GetLowStockAlertsAsync(
            int storeId,
            int page = 1,
            int pageSize = 10);
    }
}
