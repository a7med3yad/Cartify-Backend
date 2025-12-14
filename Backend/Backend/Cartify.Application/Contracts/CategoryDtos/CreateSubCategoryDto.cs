using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartify.Application.Contracts.CategoryDtos
{
    public class CreateSubCategoryDto
    {
        [Required(ErrorMessage = "Subcategory name is required")]
        public string SubCategoryName { get; set; } = string.Empty;

        public string? SubCategoryDescription { get; set; }

        [Required(ErrorMessage = "Category ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Category ID must be greater than 0")]
        public int CategoryId { get; set; }

        public IFormFile? Image { get; set; }
    }



}
