using Cartify.Domain.Interfaces.Repositories;
using Cartify.Domain.Models;
using Cartify.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Cartify.Infrastructure.Implementation.Repository
{
    public class OrdertrackingRepository : IOrdertrackingRepository
    {
        private readonly AppDbContext _context;

        public OrdertrackingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TblOrder>> GetUserOrdersAsync(int userId)
        {
            return await _context.TblOrders
                .Include(o => o.TblOrderDetails)
                    .ThenInclude(od => od.Product)
                .Include(o => o.OrderStatues)
                .Include(o => o.PaymentType)
                .Include(o => o.ShipmentMethod)
                .Where(o => o.CustomerId == userId && !o.IsDeleted)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<TblOrder> GetOrderByIdAsync(string orderId)
        {
            return await _context.TblOrders
                .Include(o => o.TblOrderDetails)
                    .ThenInclude(od => od.Product)
                .Include(o => o.OrderStatues)
                .Include(o => o.PaymentType)
                .Include(o => o.ShipmentMethod)
                .FirstOrDefaultAsync(o => o.OrderId == orderId && !o.IsDeleted);
        }

        public async Task<bool> CancelOrderAsync(string orderId)
        {
            var order = await _context.TblOrders
                .FirstOrDefaultAsync(o => o.OrderId == orderId && !o.IsDeleted);

            if (order == null) return false;

            order.OrderStatuesId = 5;
            order.UpdatedBy = order.CustomerId;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task UpdateOrderAsync(TblOrder order)
        {
            _context.TblOrders.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}