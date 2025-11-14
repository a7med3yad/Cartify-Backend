using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartify.Application.Contracts.ProductDtos
{
    public class UpdateProductDetailDto
    {
        [Required(ErrorMessage = "Product detail ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Product detail ID must be greater than 0")]
        public int ProductDetailId { get; set; }

        // All other fields are optional for update operations
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal? Price { get; set; }

        public string? Description { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be 0 or greater")]
        public int? QuantityAvailable { get; set; }

        public List<ProductAttributeDto>? Attributes { get; set; }
    }

}
