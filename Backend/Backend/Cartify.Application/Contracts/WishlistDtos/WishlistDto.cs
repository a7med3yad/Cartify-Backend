using System.ComponentModel.DataAnnotations;

namespace Cartify.Application.Contracts.WishlistDtos
{
    public class AddWishlistItemDto
    {
        [Required(ErrorMessage = "User ID is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Product detail ID is required")]
        public int ProductDetailId { get; set; }
    }
}

