using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartify.Application.Contracts.CategoryDtos
{
    public class UpdateCategoryDto
    {
        // All fields are optional for update operations
        public string? CategoryName { get; set; }
        public string? CategoryDescription { get; set; }
        public string? ImageUrl { get; set; }
    }
}
