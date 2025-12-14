using Cartify.Application.Contracts.InventoryDtos;
using Cartify.Application.Services.Interfaces.Merchant;
using Cartify.Infrastructure.Implementation.Repository;

namespace Cartify.Application.Services.Implementation.Merchant
{
    public class MerchantInventoryServices : IMerchantInventoryServices
    {
        public Task<InventoryDto?> GetInventoryByProductDetailIdAsync(int productDetailId)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<InventoryDto>> GetInventoryByStoreIdAsync(int storeId, int page = 1, int pageSize = 10)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<InventoryDto>> GetLowStockAlertsAsync(int storeId, int page = 1, int pageSize = 10)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateStockQuantityAsync(int productDetailId, int newQuantity)
        {
            throw new NotImplementedException();
        }
    }

}
