using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartify.Application.Contracts.CategoryDtos
{
    public class CreateSubCategoryDto
    {
        public string SubCategoryName { get; set; } = string.Empty;
        public string? SubCategoryDescription { get; set; }
        public int CategoryId { get; set; }
        public IFormFile? Image { get; set; }
    }



}
