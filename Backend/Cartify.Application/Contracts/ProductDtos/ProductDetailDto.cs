using Cartify.Application.Contracts.AttributeDtos;
using Cartify.Application.Contracts.PromotionDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartify.Application.Contracts.ProductDtos {
    public class ProductDetailDto
    {
        public int ProductDetailId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public string Serial { get; set; }
        public decimal? PriceAfterDiscount { get; set; }  
        public int QuantityAvailable { get; set; }
        public List<string> Images { get; set; }
        public List<AttributeDto> Attributes { get; set; }
        public List<PromotionDto> Promotions { get; set; }
    }

}
