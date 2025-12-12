using System.Collections.Generic;
using System.Threading.Tasks;
using Cartify.Application.Contracts.CartDtos;

namespace Cartify.Application.Services.Interfaces.Product
{
    public interface ICartService
    {
        Task AddToCartAsync(AddCartItemDto dto);

        Task RemoveFromCartAsync(int userId, int productDetailId);

        Task<IEnumerable<AddCartItemDto>> GetCartByUserIdAsync(int userId);

        Task UpdateCartItemQuantityAsync(UpdateCartItemQuantityDto dto);
    }
}
