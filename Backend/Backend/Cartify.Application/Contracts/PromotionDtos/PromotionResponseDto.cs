using System;

namespace Cartify.Application.Contracts.PromotionDtos
{
    public class PromotionResponseDto
    {
        public int PromotionId { get; set; }
        public string PromotionName { get; set; }
        public decimal DiscountPercentage { get; set; }
        public string? ImgUrl { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}

