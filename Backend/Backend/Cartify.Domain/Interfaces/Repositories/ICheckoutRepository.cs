using Cartify.Domain.Models;

namespace Cartify.Domain.Interfaces.Repositories
{
    public interface ICheckoutRepository
    {
        Task<TblOrder> CreateOrderAsync(TblOrder order);
        Task UpdateProductQuantityAsync(int productId, int quantity);
        Task<TblProduct> GetProductByIdAsync(int productId);
    }
}