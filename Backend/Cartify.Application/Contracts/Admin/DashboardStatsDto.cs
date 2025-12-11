namespace Cartify.Application.Contracts.Admin
{
    public class DashboardStatsDto
    {
        public int TotalUsers { get; set; }
        public int TotalStores { get; set; }
        public int TotalProducts { get; set; }
        public int TotalOrders { get; set; }
        public decimal TodayRevenue { get; set; }
        public int TodayOrders { get; set; }
        public int NewUsersToday { get; set; }
        public int OpenTickets { get; set; }
        public List<RecentActivityDto> RecentActivities { get; set; }
    }

    public class RecentActivityDto
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
    }
}


