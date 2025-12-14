using System.ComponentModel.DataAnnotations;

namespace Cartify.Application.Contracts.OrderDtos
{
    public class CheckoutDto
    {
        [Required(ErrorMessage = "User ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "User ID must be greater than 0")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Cart items are required")]
        [MinLength(1, ErrorMessage = "At least one cart item is required")]
        public List<CartItemDto> CartItems { get; set; } = new List<CartItemDto>();

        [Required(ErrorMessage = "Shipping information is required")]
        public ShippingInfoDto ShippingInfo { get; set; } = new ShippingInfoDto();

        [Required(ErrorMessage = "Payment information is required")]
        public PaymentInfoDto PaymentInfo { get; set; } = new PaymentInfoDto();
    }

    public class CartItemDto
    {
        [Required(ErrorMessage = "Product ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Product ID must be greater than 0")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }
    }

    public class ShippingInfoDto
    {
        [Required(ErrorMessage = "Full name is required")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; } = string.Empty;

        [Required(ErrorMessage = "Postal code is required")]
        public string PostalCode { get; set; } = string.Empty;
    }

    public class PaymentInfoDto
    {
        [Required(ErrorMessage = "Payment method is required")]
        public string PaymentMethod { get; set; } = string.Empty; // "card", "paypal", "cod"

        // Card fields are optional (only required if PaymentMethod is "card")
        public string? CardNumber { get; set; }
        public string? CardHolder { get; set; }
        public string? ExpiryDate { get; set; }
        public string? CVV { get; set; }
    }
}

