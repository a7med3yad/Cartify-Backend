using Cartify.Application.Contracts.OrderDtos;
using Cartify.Infrastructure.Implementation.Repository;
using System;
using System.Threading.Tasks;

namespace Cartify.Application.Services.Interfaces.Merchant
{
    public interface IMerchantOrderServices
    {
        Task<PagedResult<OrderDetailDto>> GetOrdersByStoreIdAsync(int storeId, int page = 1, int pageSize = 10);
        Task<OrderDetailDto?> GetOrderByIdAsync(string orderId);
        Task<bool> UpdateOrderStatusAsync(string orderId, string newStatus);
        Task<PagedResult<OrderDto>> FilterOrdersAsync(int storeId, DateTime? startDate, DateTime? endDate, string? status, int page = 1, int pageSize = 10);
    }
}
