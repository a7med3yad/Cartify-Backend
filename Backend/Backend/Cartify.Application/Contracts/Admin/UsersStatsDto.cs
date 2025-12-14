namespace Cartify.Application.Contracts.Admin
{
    public class UsersStatsDto
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int SuspendedUsers { get; set; }
        public int NewUsersThisMonth { get; set; }
        public List<UserRoleCountDto> UsersByRole { get; set; }
    }

    public class UserRoleCountDto
    {
        public string Role { get; set; }
        public int Count { get; set; }
    }
}


