using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartify.Application.Contracts.ProductDtos
{
 
    public class ProductAttributeDto
    {
        [Required(ErrorMessage = "Attribute ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Attribute ID must be greater than 0")]
        public int AttributeId { get; set; }

        [Required(ErrorMessage = "Measure unit ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Measure unit ID must be greater than 0")]
        public int MeasureUnitId { get; set; }
    }
}
