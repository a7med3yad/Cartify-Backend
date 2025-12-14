using Cartify.Application.Contracts.OrderDtos;
using Cartify.Domain.Models;

namespace Cartify.Application.Services.Interfaces
{
    public interface ICheckoutService
    {
        Task<TblOrder> ProcessCheckoutAsync(CheckoutDto checkoutData);
    }
}