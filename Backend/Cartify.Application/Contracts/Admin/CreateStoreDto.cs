using System.ComponentModel.DataAnnotations;

namespace Cartify.Application.Contracts.Admin
{
    public class CreateStoreDto
    {
        [Required]
        public string StoreName { get; set; }

        [Required]
        public string OwnerEmail { get; set; }

        public string Category { get; set; }
        public string Location { get; set; }
    }
}


