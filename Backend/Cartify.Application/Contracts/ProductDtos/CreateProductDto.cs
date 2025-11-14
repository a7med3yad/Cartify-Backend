using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartify.Application.Contracts.ProductDtos
{
    public class CreateProductDto
    {
        [Required(ErrorMessage = "Product name is required")]
        public string ProductName { get; set; } = string.Empty;

        public string? ProductDescription { get; set; }

        [Required(ErrorMessage = "Type ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Type ID must be greater than 0")]
        public int TypeId { get; set; }

        [Required(ErrorMessage = "Store ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Store ID must be greater than 0")]
        public int StoreId { get; set; }
    }
}
