using Cartify.Domain.Interfaces.Repositories;
using Cartify.Domain.Models;
using Cartify.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Cartify.Infrastructure.Implementation.Repository
{
    public class CheckoutRepository : ICheckoutRepository
    {
        private readonly AppDbContext _context;

        public CheckoutRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TblOrder> CreateOrderAsync(TblOrder order)
        {
            _context.TblOrders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task UpdateProductQuantityAsync(int productId, int quantity)
        {
            var product = await _context.TblProducts.FindAsync(productId);
            if (product != null)
            {
                // هنا محتاج نحدد إزاي نupdate الكمية
                // حسب الـ inventory system عندك
                await _context.SaveChangesAsync();
            }
        }

        public async Task<TblProduct> GetProductByIdAsync(int productId)
        {
            return await _context.TblProducts
                .FirstOrDefaultAsync(p => p.ProductId == productId && !p.IsDeleted);
        }
    }
}