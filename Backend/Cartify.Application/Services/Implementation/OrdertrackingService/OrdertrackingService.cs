using Cartify.Application.Services.Interfaces;
using Cartify.Domain.Interfaces.Repositories;
using Cartify.Domain.Models;

namespace Cartify.Application.Services.Implementation
{
    public class OrdertrackingService : IOrdertrackingservice
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrdertrackingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<TblOrder>> GetUserOrdersAsync(int userId)
        {
            return await _unitOfWork.OrdertrackingRepository.GetUserOrdersAsync(userId);
        }

        public async Task<TblOrder> GetOrderDetailsAsync(string orderId)
        {
            return await _unitOfWork.OrdertrackingRepository.GetOrderByIdAsync(orderId);
        }

        public async Task<bool> CancelOrderAsync(string orderId)
        {
            return await _unitOfWork.OrdertrackingRepository.CancelOrderAsync(orderId);
        }
    }
}