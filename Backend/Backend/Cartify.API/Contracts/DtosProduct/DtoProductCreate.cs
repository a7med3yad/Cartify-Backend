using System.ComponentModel.DataAnnotations;

namespace Cartify.API.Contracts.ProductDtos
{
	public class DtoProductCreate
    {
        [Required(ErrorMessage = "Product name is required")]
        public string ProductName { get; set; } = string.Empty;

        public string? ProductDescription { get; set; }

        [Required(ErrorMessage = "Type ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Type ID must be greater than 0")]
        public int TypeId { get; set; }
    }
}
