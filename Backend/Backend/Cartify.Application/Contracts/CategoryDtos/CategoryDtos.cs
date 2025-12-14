using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartify.Application.Contracts.CategoryDtos
{
        public class CategoryDto
        {
            public int CategoryId { get; set; }
            public string CategoryName { get; set; }
            public string CategoryDescription { get; set; }
            public string? ImageUrl { get; set; }
            public int ProductCount { get; set; }
        }

     

}
