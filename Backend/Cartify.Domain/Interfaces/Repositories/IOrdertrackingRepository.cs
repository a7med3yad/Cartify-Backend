using Cartify.Domain.Models;

namespace Cartify.Domain.Interfaces.Repositories
{
    public interface IOrdertrackingRepository
    {
        Task<List<TblOrder>> GetUserOrdersAsync(int userId);
        Task<TblOrder> GetOrderByIdAsync(string orderId);
        Task<bool> CancelOrderAsync(string orderId);
        Task UpdateOrderAsync(TblOrder order);
    }
}