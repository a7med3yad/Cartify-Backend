namespace Cartify.Application.Contracts.Admin
{
    public class TopCategoryDto
    {
        public string CategoryName { get; set; }
        public int OrderCount { get; set; }
        public decimal TotalRevenue { get; set; }
        public int Percentage { get; set; }
    }
}


