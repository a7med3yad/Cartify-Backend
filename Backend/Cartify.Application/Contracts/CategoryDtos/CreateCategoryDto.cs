using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartify.Application.Contracts.CategoryDtos
{
    public class CreateCategoryDto
    {
        [Required(ErrorMessage = "Category name is required")]
        public string CategoryName { get; set; } = string.Empty;

        public string? CategoryDescription { get; set; }
        
        public IFormFile? Image { get; set; }
    }


}
