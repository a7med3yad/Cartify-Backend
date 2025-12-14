using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartify.Application.Contracts.PromotionDtos
{
    public class UpdatePromotionDto
    {
        public string? PromotionName { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public string? ImgUrl { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
