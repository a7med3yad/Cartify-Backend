using System.ComponentModel.DataAnnotations;

namespace Cartify.Application.Contracts.CartDtos
{
    public class AddCartItemDto
    {
        [Required(ErrorMessage = "User ID is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Product detail ID is required")]
        public int ProductDetailId { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
    }

    public class UpdateCartItemQuantityDto
    {
        [Required(ErrorMessage = "Cart item ID is required")]
        public int CartItemId { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
    }
}

