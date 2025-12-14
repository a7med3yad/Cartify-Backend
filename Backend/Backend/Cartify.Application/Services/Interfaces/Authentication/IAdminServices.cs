using Cartify.Application.Contracts.Admin;
using Cartify.Application.Contracts.OrderDtos;

namespace Cartify.Application.Services.Interfaces
{
    public interface IAdminService
    {
        // User Management
        Task<PagedResultDto<AdminUserDto>> GetAllUsersAsync(int page, int pageSize);
        Task<AdminUserDto> GetUserByIdAsync(string id);
        Task<ApiResultDto> CreateUserAsync(CreateUserDto dto);
        Task<ApiResultDto> UpdateUserAsync(string id, UpdateUserDto dto);
        Task<ApiResultDto> DeleteUserAsync(string id);
        Task<ApiResultDto> SuspendUserAsync(string id);
        Task<ApiResultDto> ActivateUserAsync(string id);

        // Store Management
        Task<PagedResultDto<AdminStoreDto>> GetAllStoresAsync(int page, int pageSize);
        Task<AdminStoreDto> GetStoreByIdAsync(int id);
        Task<ApiResultDto> CreateStoreAsync(CreateStoreDto dto);
        Task<ApiResultDto> UpdateStoreAsync(int id, UpdateStoreDto dto);
        Task<ApiResultDto> DeleteStoreAsync(int id);

        // Order Management
        Task<PagedResultDto<AdminOrderDto>> GetAllOrdersAsync(int page, int pageSize);
        Task<AdminOrderDto> GetOrderByIdAsync(int id);
        Task<ApiResultDto> UpdateOrderStatusAsync(int id, UpdateOrderStatusDto dto);
        Task<ApiResultDto> CancelOrderAsync(int id);

        // Analytics & Reports
        Task<DashboardStatsDto> GetDashboardStatsAsync();
        Task<List<SalesDataDto>> GetSalesDataAsync(DateTime? startDate, DateTime? endDate);
        Task<List<TopCategoryDto>> GetTopCategoriesAsync();
        Task<List<AdminOrderDto>> GetLatestOrdersAsync(int count);
        Task<UsersStatsDto> GetUsersStatsAsync();
    }
}


