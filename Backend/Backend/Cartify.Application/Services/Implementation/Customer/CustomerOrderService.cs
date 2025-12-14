using Cartify.Application.Contracts.OrderDtos;
using Cartify.Application.Services.Interfaces.Customer;
using Cartify.Domain.Interfaces.Repositories;
using Cartify.Domain.Models;
using Cartify.Infrastructure.Implementation.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Cartify.Application.Services.Implementation.Customer
{
    public class CustomerOrderService : ICustomerOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerOrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // =========================================================
        // 🔹 CREATE ORDER
        // =========================================================
        public async Task<OrderResponseDto?> CreateOrderAsync(int customerId, CreateOrderDto dto)
        {
            // Validate store exists
            var store = await _unitOfWork.UserStorerepository.ReadByIdAsync(dto.StoreId);
            if (store == null)
                return null;

            // Get default order status (usually "Pending" or "Processing")
            var defaultStatus = await _unitOfWork.OrderStatusRepository
                .GetAllIncluding2()
                .Where(s => s.Name.ToLower().Contains("pending") || s.Name.ToLower().Contains("processing"))
                .FirstOrDefaultAsync();

            if (defaultStatus == null)
            {
                // Try to get first status
                defaultStatus = await _unitOfWork.OrderStatusRepository.GetAllIncluding2().FirstOrDefaultAsync();
                if (defaultStatus == null)
                    return null;
            }

            // Generate order ID
            var orderId = GenerateOrderId();

            // Calculate order totals
            decimal totalPrice = 0;
            decimal totalDiscount = 0;
            var orderDetails = new List<TblOrderDetail>();

            foreach (var item in dto.OrderItems)
            {
                // Get product detail
                var productDetail = await _unitOfWork.ProductDetailsRepository.ReadByIdAsync(item.ProductDetailId);
                if (productDetail == null || productDetail.IsDeleted)
                    return null;

                // Check inventory
                var inventory = await _unitOfWork.InventoryRepository
                    .GetAllIncluding2()
                    .Where(i => i.ProductDetailId == item.ProductDetailId)
                    .FirstOrDefaultAsync();

                if (inventory == null || inventory.QuantityAvailable < item.Quantity)
                    return null;

                // Calculate item price and discount
                var itemPrice = productDetail.Price * item.Quantity;
                decimal itemDiscount = 0;


                totalPrice += itemPrice;
                totalDiscount += itemDiscount;

                // Create order detail
                var orderDetail = new TblOrderDetail
                {
                    OrderId = orderId,
                    ProductId = productDetail.ProductId,
                    Quantity = item.Quantity,
                    Price = itemPrice - itemDiscount,
                    Discount = itemDiscount,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false
                };

                orderDetails.Add(orderDetail);

                // Update inventory
                inventory.QuantityAvailable -= item.Quantity;
                _unitOfWork.InventoryRepository.Update(inventory);
            }

            // Calculate tax and grand total
            var tax = dto.Tax ?? 0;
            var grandTotal = totalPrice - totalDiscount + tax;

            // Create order
            var order = new TblOrder
            {
                OrderId = orderId,
                CustomerId = customerId,
                StoreId = dto.StoreId,
                PaymentTypeId = dto.PaymentTypeId,
                ShipmentMethodId = dto.ShipmentMethodId,
                OrderStatuesId = defaultStatus.OrderStatuesId,
                TotalPrice = totalPrice,
                TotalDiscount = totalDiscount,
                Tax = tax,
                GrantTotal = grandTotal,
                OrderDate = DateTime.Now,
                CreatedDate = DateTime.Now,
                IsDeleted = false
            };

            await _unitOfWork.OrderRepository.CreateAsync(order);

            // Create order details
            foreach (var detail in orderDetails)
            {
                await _unitOfWork.OrderDetailsRepository.CreateAsync(detail);
            }

            var saved = await _unitOfWork.SaveChanges();
            if (saved <= 0)
                return null;

            // Return order response
            return await GetOrderByIdAsync(orderId, customerId);
        }

        // =========================================================
        // 🔹 GET ORDER BY ID
        // =========================================================
        public async Task<OrderResponseDto?> GetOrderByIdAsync(string orderId, int customerId)
        {
            var order = await _unitOfWork.OrderRepository
                .GetAllIncluding2()
                .Where(o => o.OrderId == orderId && o.CustomerId == customerId && !o.IsDeleted)
                .Include(o => o.OrderStatues)
                .Include(o => o.PaymentType)
                .Include(o => o.ShipmentMethod)
                .Include(o => o.UserStore)
                .Include(o => o.TblOrderDetails)
                    .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync();

            if (order == null)
                return null;

            return MapToOrderResponseDto(order);
        }

        // =========================================================
        // 🔹 GET CUSTOMER ORDERS
        // =========================================================
        public async Task<PagedResult<OrderResponseDto>> GetCustomerOrdersAsync(
            int customerId,
            int page = 1,
            int pageSize = 10)
        {
            var ordersQuery = _unitOfWork.OrderRepository
                .GetAllIncluding2()
                .Where(o => o.CustomerId == customerId && !o.IsDeleted)
                .Include(o => o.OrderStatues)
                .Include(o => o.PaymentType)
                .Include(o => o.ShipmentMethod)
                .Include(o => o.UserStore)
                .AsQueryable();

            if (ordersQuery == null || !ordersQuery.Any())
                return new PagedResult<OrderResponseDto>(new List<OrderResponseDto>(), 0, page, pageSize);

            var totalOrders = ordersQuery.Count();

            var pagedOrders = ordersQuery
                .OrderByDescending(o => o.OrderDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var orderDtos = pagedOrders.Select(MapToOrderResponseDto).ToList();

            return new PagedResult<OrderResponseDto>(orderDtos, totalOrders, page, pageSize);
        }

        // =========================================================
        // 🔹 CANCEL ORDER
        // =========================================================
        public async Task<bool> CancelOrderAsync(string orderId, int customerId)
        {
            var order = await _unitOfWork.OrderRepository
                .GetAllIncluding2()
                .Where(o => o.OrderId == orderId && o.CustomerId == customerId && !o.IsDeleted)
                .FirstOrDefaultAsync();

            if (order == null)
                return false;

            // Check if order can be cancelled (usually only pending or processing orders)
            var status = await _unitOfWork.OrderStatusRepository.ReadByIdAsync(order.OrderStatuesId);
            if (status == null)
                return false;

            var statusName = status.Name.ToLower();
            if (statusName.Contains("cancelled") || statusName.Contains("delivered") || statusName.Contains("completed"))
                return false;

            // Get cancelled status
            var cancelledStatus = await _unitOfWork.OrderStatusRepository
                .GetAllIncluding2()
                .Where(s => s.Name.ToLower().Contains("cancel"))
                .FirstOrDefaultAsync();

            if (cancelledStatus == null)
                return false;

            order.OrderStatuesId = cancelledStatus.OrderStatuesId;
            order.UpdatedBy = customerId;
            _unitOfWork.OrderRepository.Update(order);

            // Restore inventory
            var orderDetails = await _unitOfWork.OrderDetailsRepository
                .GetAllIncluding2()
                .Where(od => od.OrderId == orderId)
                .ToListAsync();

            foreach (var detail in orderDetails)
            {
                // Note: This is simplified - you might need to track which ProductDetailId was used
                // For now, we'll skip inventory restoration as we don't have ProductDetailId in OrderDetail
                // You may need to add ProductDetailId to TblOrderDetail or track it differently
            }

            var saved = await _unitOfWork.SaveChanges();
            return saved > 0;
        }

        // =========================================================
        // 🔹 HELPER METHODS
        // =========================================================
        private string GenerateOrderId()
        {
            return $"ORD-{DateTime.Now:yyyyMMddHHmmss}-{Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()}";
        }

        private OrderResponseDto MapToOrderResponseDto(TblOrder order)
        {
            return new OrderResponseDto
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                StatusName = order.OrderStatues?.Name ?? "Unknown",
                StatusId = order.OrderStatuesId,
                StoreName = order.UserStore?.StoreName ?? "Unknown Store",
                StoreId = order.StoreId,
                PaymentType = order.PaymentType?.Name ?? "Unknown",
                ShipmentMethod = order.ShipmentMethod?.Name ?? "Unknown",
                TotalPrice = order.TotalPrice,
                TotalDiscount = order.TotalDiscount,
                Tax = order.Tax,
                GrantTotal = order.GrantTotal,
                Items = order.TblOrderDetails?
                    .Select(od => new OrderItemDto
                    {
                        ProductId = od.ProductId,
                        ProductName = od.Product?.ProductName ?? "Unknown Product",
                        Quantity = od.Quantity,
                        Price = od.Price,
                        ImageUrl = null // You might want to get this from product images
                    })
                    .ToList() ?? new List<OrderItemDto>()
            };
        }
    }
}

