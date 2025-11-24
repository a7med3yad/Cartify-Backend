using System.Collections.Generic;
using System.Threading.Tasks;
using Cartify.Application.Contracts.WishlistDtos;

namespace Cartify.Application.Services.Interfaces.Product
{
    public interface IWishListService
    {
        Task AddToWishListAsync(int userId, int productId);

        Task RemoveFromWishListAsync(int userId, int productId);

        Task<IEnumerable<AddWishlistItemDto>> GetWishListByUserIdAsync(int userId);
    }
}