namespace Cartify.Application.Contracts.ProfileDtos
{
    public class MerchantProfileDto
    {
        public string UserId { get; set; }
        public string StoreName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? LogoUrl { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
    }

}
