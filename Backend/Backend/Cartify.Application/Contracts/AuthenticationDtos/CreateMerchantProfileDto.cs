using System.ComponentModel.DataAnnotations;

namespace Cartify.Application.Contracts.AuthenticationDtos
{
    public class ResetPasswordEmailDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
    }

    public class CreateMerchantProfileDto
    {
        [Required(ErrorMessage = "Store name is required")]
        [StringLength(100, ErrorMessage = "Store name cannot exceed 100 characters")]
        [MinLength(3, ErrorMessage = "Store name must be at least 3 characters")]
        public string StoreName { get; set; }
    }
}

