using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartify.Application.Contracts.AttributeDtos
{
    public class AttributeDto
    {
        [Required(ErrorMessage = "Attribute name is required")]
        public string Name { get; set; } = string.Empty;

        public string? MeasureUnit { get; set; }
    }

}
