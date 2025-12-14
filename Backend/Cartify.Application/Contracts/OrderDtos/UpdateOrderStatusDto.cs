using System.ComponentModel.DataAnnotations;

namespace Cartify.Application.Contracts.OrderDtos
{
    public class UpdateOrderStatusDto
    {
        [Required(ErrorMessage = "Status is required")]
        [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters")]
        public string Status { get; set; }
    }
}

