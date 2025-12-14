using Cartify.Application.Contracts.OrderDtos;
using Cartify.Infrastructure.Implementation.Repository;
using System;
using System.Threading.Tasks;

namespace Cartify.Application.Services.Interfaces.Customer
{
    public interface ICustomerOrderService
    {
        Task<OrderResponseDto?> CreateOrderAsync(int customerId, CreateOrderDto dto);
        Task<OrderResponseDto?> GetOrderByIdAsync(string orderId, int customerId);
        Task<PagedResult<OrderResponseDto>> GetCustomerOrdersAsync(int customerId, int page = 1, int pageSize = 10);
        Task<bool> CancelOrderAsync(string orderId, int customerId);
    }
}

