using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartify.Application.Contracts.PromotionDtos
{
    public class CreatePromotionDto
    {
        [Required(ErrorMessage = "Promotion name is required")]
        public string PromotionName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Discount percentage is required")]
        [Range(0, 100, ErrorMessage = "Discount percentage must be between 0 and 100")]
        public decimal DiscountPercentage { get; set; }

        public string? ImgUrl { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        public DateTime EndDate { get; set; }
    }

}
