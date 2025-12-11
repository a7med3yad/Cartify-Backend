namespace Cartify.Application.Contracts.Admin
{
    public class AdminStoreDto
    {
        public int Id { get; set; }
        public string StoreName { get; set; }
        public string OwnerEmail { get; set; }
        public string OwnerName { get; set; }
        public string Category { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int ProductCount { get; set; }
    }
}


