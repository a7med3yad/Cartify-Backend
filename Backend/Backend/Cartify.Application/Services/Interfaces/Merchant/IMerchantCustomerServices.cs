using Cartify.Application.Contracts.CustomerDtos;
using Cartify.Infrastructure.Implementation.Repository;
using System.Threading.Tasks;

namespace Cartify.Application.Services.Interfaces.Merchant
{
    public interface IMerchantCustomerServices
    {
        Task<PagedResult<CustomerDto>> GetCustomersByStoreIdAsync(int storeId, int page = 1, int pageSize = 10);
        Task<CustomerDto?> GetCustomerByIdAsync(string userId);
        Task<int> GetCustomerCountAsync(int storeId);
    }
}
