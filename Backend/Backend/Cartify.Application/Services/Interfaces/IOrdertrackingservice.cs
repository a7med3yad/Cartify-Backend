using Cartify.Domain.Models;

namespace Cartify.Application.Services.Interfaces
{
    public interface IOrdertrackingservice
    {
        Task<List<TblOrder>> GetUserOrdersAsync(int userId);
        Task<TblOrder> GetOrderDetailsAsync(string orderId);
        Task<bool> CancelOrderAsync(string orderId);
    }
}