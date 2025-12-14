using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Cartify.Application.Contracts.ProductDtos
{
    public class UpdateProductDto
    {
        public string? ProductName { get; set; }

        public string? ProductDescription { get; set; }

        public int? TypeId { get; set; }

        public List<IFormFile>? NewImages { get; set; }

        public int? UpdatedBy { get; set; }
    }
}
