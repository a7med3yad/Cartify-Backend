using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartify.Application.Contracts.ProfileDtos
{
    public class UpdateStoreInfoDto
    {
        [Required(ErrorMessage = "Store ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Store ID must be greater than 0")]
        public int StoreId { get; set; }

        [Required(ErrorMessage = "Store name is required")]
        public string StoreName { get; set; } = string.Empty;

        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
    }
}
