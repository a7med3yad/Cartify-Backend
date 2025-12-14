using System;
using System.Collections.Generic;

namespace Cartify.Application.Contracts.ProductDtos
{
    public class ProductResponseDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string TypeName { get; set; }
        public string CategoryName { get; set; }
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public List<string> ImageUrls { get; set; } = new List<string>();
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<ProductDetailDto> ProductDetails { get; set; } = new List<ProductDetailDto>();
    }
}

