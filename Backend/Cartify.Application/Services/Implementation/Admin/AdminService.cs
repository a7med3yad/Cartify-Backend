using Cartify.Application.Contracts.Admin;
using Cartify.Application.Contracts.OrderDtos;
using Cartify.Application.Services.Interfaces;
using Cartify.Domain.Entities;
using Cartify.Domain.Interfaces.Repositories;
using Cartify.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Cartify.Application.Services.Implementation.Admin
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<TblUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminService(IUnitOfWork unitOfWork, UserManager<TblUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        #region User Management

        public async Task<PagedResultDto<AdminUserDto>> GetAllUsersAsync(int page, int pageSize)
        {
            var users = await _userManager.Users
                .Where(u => !u.IsDeleted)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new AdminUserDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    UserName = u.UserName,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    PhoneNumber = u.PhoneNumber,
                    IsActive = u.LockoutEnd == null || u.LockoutEnd < DateTimeOffset.UtcNow,
                    CreatedDate = u.CreatedDate,
                    IsDeleted = u.IsDeleted
                })
                .ToListAsync();

            var totalCount = await _userManager.Users.Where(u => !u.IsDeleted).CountAsync();

            // Get roles for each user
            foreach (var user in users)
            {
                var dbUser = await _userManager.FindByIdAsync(user.Id);
                if (dbUser != null)
                {
                    var roles = await _userManager.GetRolesAsync(dbUser);
                    user.Role = roles.FirstOrDefault() ?? "User";
                }
            }

            return new PagedResultDto<AdminUserDto>
            {
                Items = users,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<AdminUserDto> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null || user.IsDeleted)
                return null;

            var roles = await _userManager.GetRolesAsync(user);

            return new AdminUserDto
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Role = roles.FirstOrDefault() ?? "User",
                IsActive = user.LockoutEnd == null || user.LockoutEnd < DateTimeOffset.UtcNow,
                CreatedDate = user.CreatedDate,
                IsDeleted = user.IsDeleted
            };
        }

        public async Task<ApiResultDto> CreateUserAsync(CreateUserDto dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
                return new ApiResultDto { Success = false, Message = "User already exists" };

            var user = new TblUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                CreatedDate = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return new ApiResultDto { Success = false, Message = string.Join(", ", result.Errors.Select(e => e.Description)) };

            // Add to role
            if (!string.IsNullOrEmpty(dto.Role))
            {
                await _userManager.AddToRoleAsync(user, dto.Role);
            }

            return new ApiResultDto { Success = true, Message = "User created successfully" };
        }

        public async Task<ApiResultDto> UpdateUserAsync(string id, UpdateUserDto dto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null || user.IsDeleted)
                return new ApiResultDto { Success = false, Message = "User not found" };

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.PhoneNumber = dto.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return new ApiResultDto { Success = false, Message = string.Join(", ", result.Errors.Select(e => e.Description)) };

            // Update role 
            if (!string.IsNullOrEmpty(dto.Role))
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRoleAsync(user, dto.Role);
            }

            return new ApiResultDto { Success = true, Message = "User updated successfully" };
        }

        public async Task<ApiResultDto> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null || user.IsDeleted)
                return new ApiResultDto { Success = false, Message = "User not found" };

            user.IsDeleted = true;
            user.DeletedDate = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return new ApiResultDto { Success = true, Message = "User deleted successfully" };
        }

        public async Task<ApiResultDto> SuspendUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null || user.IsDeleted)
                return new ApiResultDto { Success = false, Message = "User not found" };

            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(100));
            return new ApiResultDto { Success = true, Message = "User suspended successfully" };
        }

        public async Task<ApiResultDto> ActivateUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null || user.IsDeleted)
                return new ApiResultDto { Success = false, Message = "User not found" };

            await _userManager.SetLockoutEndDateAsync(user, null);
            return new ApiResultDto { Success = true, Message = "User activated successfully" };
        }

        #endregion

        #region Store Management

        public async Task<PagedResultDto<AdminStoreDto>> GetAllStoresAsync(int page, int pageSize)
        {
            var query = _unitOfWork.UserStorerepository
                .GetAll()
                .Where(s => !s.IsDeleted)
                .Include(s => s.Merchant)
                .Include(s => s.TblProducts);

            var totalCount = await query.CountAsync();

            var stores = await query
                .OrderByDescending(s => s.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(s => new AdminStoreDto
                {
                    Id = s.UserStoreId,
                    StoreName = s.StoreName,
                    OwnerEmail = s.Merchant != null ? s.Merchant.Email : string.Empty,
                    OwnerName = s.Merchant != null
                        ? string.Join(" ", new[] { s.Merchant.FirstName, s.Merchant.LastName }.Where(n => !string.IsNullOrWhiteSpace(n)))
                        : string.Empty,
                    CreatedDate = s.CreatedDate,
                    ProductCount = s.TblProducts.Count(p => !p.IsDeleted),
                    Status = s.IsDeleted ? "Inactive" : "Active"
                })
                .ToListAsync();

            return new PagedResultDto<AdminStoreDto>
            {
                Items = stores,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<AdminStoreDto> GetStoreByIdAsync(int id)
        {
            var store = await _unitOfWork.UserStorerepository
                .GetAll()
                .Include(s => s.Merchant)
                .Include(s => s.TblProducts)
                .FirstOrDefaultAsync(s => s.UserStoreId == id && !s.IsDeleted);

            if (store == null)
                return null;

            return new AdminStoreDto
            {
                Id = store.UserStoreId,
                StoreName = store.StoreName,
                OwnerEmail = store.Merchant?.Email ?? string.Empty,
                OwnerName = store.Merchant != null
                    ? string.Join(" ", new[] { store.Merchant.FirstName, store.Merchant.LastName }.Where(n => !string.IsNullOrWhiteSpace(n)))
                    : string.Empty,
                CreatedDate = store.CreatedDate,
                ProductCount = store.TblProducts.Count(p => !p.IsDeleted),
                Status = store.IsDeleted ? "Inactive" : "Active"
            };
        }

        public async Task<ApiResultDto> CreateStoreAsync(CreateStoreDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.OwnerEmail);
            if (user == null)
                return new ApiResultDto { Success = false, Message = "User not found" };

            var hasStore = await _unitOfWork.UserStorerepository
                .GetAll()
                .AnyAsync(s => s.MerchantId == user.Id && !s.IsDeleted);

            if (hasStore)
                return new ApiResultDto { Success = false, Message = "Merchant already has an active store" };

            var store = new TblUserStore
            {
                MerchantId = user.Id,
                StoreName = dto.StoreName,
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false
            };

            await _unitOfWork.UserStorerepository.CreateAsync(store);
            await _unitOfWork.SaveChanges();

            return new ApiResultDto { Success = true, Message = "Store created successfully" };
        }

        public async Task<ApiResultDto> UpdateStoreAsync(int id, UpdateStoreDto dto)
        {
            var store = await _unitOfWork.UserStorerepository.ReadByIdAsync(id);
            if (store == null || store.IsDeleted)
                return new ApiResultDto { Success = false, Message = "Store not found" };

            store.StoreName = dto.StoreName;
            _unitOfWork.UserStorerepository.Update(store);
            await _unitOfWork.SaveChanges();

            return new ApiResultDto { Success = true, Message = "Store updated successfully" };
        }

        public async Task<ApiResultDto> DeleteStoreAsync(int id)
        {
            var store = await _unitOfWork.UserStorerepository.ReadByIdAsync(id);
            if (store == null || store.IsDeleted)
                return new ApiResultDto { Success = false, Message = "Store not found" };

            store.IsDeleted = true;
            store.DeletedDate = DateTime.UtcNow;
            _unitOfWork.UserStorerepository.Update(store);
            await _unitOfWork.SaveChanges();

            return new ApiResultDto { Success = true, Message = "Store deleted successfully" };
        }

        #endregion

        #region Order Management

        public async Task<PagedResultDto<AdminOrderDto>> GetAllOrdersAsync(int page, int pageSize)
        {
            var orders = await _unitOfWork.OrderRepository.GetAll()
                .Include(o => o.OrderStatues)
                .Include(o => o.PaymentType)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(o => new AdminOrderDto
                {
                    Id = int.Parse(o.OrderId),
                    OrderNumber = o.OrderId,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.GrantTotal,
                    Status = o.OrderStatues.Name,
                    PaymentMethod = o.PaymentType.Name
                })
                .ToListAsync();

            var totalCount = await _unitOfWork.OrderRepository.GetAll().CountAsync();

            return new PagedResultDto<AdminOrderDto>
            {
                Items = orders,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<AdminOrderDto> GetOrderByIdAsync(int id)
        {
            var order = await _unitOfWork.OrderRepository.GetAll()
                .Include(o => o.OrderStatues)
                .Include(o => o.PaymentType)
                .FirstOrDefaultAsync(o => o.OrderId == id.ToString());

            if (order == null)
                return null;

            return new AdminOrderDto
            {
                Id = int.Parse(order.OrderId),
                OrderNumber = order.OrderId,
                OrderDate = order.OrderDate,
                TotalAmount = order.GrantTotal,
                Status = order.OrderStatues.Name,
                PaymentMethod = order.PaymentType.Name
            };
        }

        public async Task<ApiResultDto> UpdateOrderStatusAsync(int id, UpdateOrderStatusDto dto)
        {
            var order = await _unitOfWork.OrderRepository
                .GetAll(false)
                .FirstOrDefaultAsync(o => o.OrderId == id.ToString());

            if (order == null)
                return new ApiResultDto { Success = false, Message = "Order not found" };

            var status = await _unitOfWork.OrderStatusRepository
                .GetAll()
                .FirstOrDefaultAsync(s => s.Name == dto.Status);

            if (status == null)
                return new ApiResultDto { Success = false, Message = "Invalid status" };

            order.OrderStatuesId = status.OrderStatuesId;
            _unitOfWork.OrderRepository.Update(order);
            await _unitOfWork.SaveChanges();

            return new ApiResultDto { Success = true, Message = "Order status updated successfully" };
        }

        public async Task<ApiResultDto> CancelOrderAsync(int id)
        {
            var order = await _unitOfWork.OrderRepository
                .GetAll(false)
                .FirstOrDefaultAsync(o => o.OrderId == id.ToString());

            if (order == null)
                return new ApiResultDto { Success = false, Message = "Order not found" };

            var status = await _unitOfWork.OrderStatusRepository
                .GetAll()
                .FirstOrDefaultAsync(s => s.Name.ToLower().Contains("cancel"));

            if (status != null)
            {
                order.OrderStatuesId = status.OrderStatuesId;
                _unitOfWork.OrderRepository.Update(order);
            }

            await _unitOfWork.SaveChanges();

            return new ApiResultDto { Success = true, Message = "Order cancelled successfully" };
        }

        #endregion

        #region Analytics & Reports

        public async Task<DashboardStatsDto> GetDashboardStatsAsync()
        {
            var today = DateTime.UtcNow.Date;

            var stats = new DashboardStatsDto
            {
                TotalUsers = await _userManager.Users.Where(u => !u.IsDeleted).CountAsync(),
                TotalStores = await _unitOfWork.UserStorerepository.GetAll().Where(s => !s.IsDeleted).CountAsync(),
                TotalProducts = await _unitOfWork.ProductRepository.GetAll().CountAsync(),
                TotalOrders = await _unitOfWork.OrderRepository.GetAll().CountAsync(),
                TodayRevenue = await _unitOfWork.OrderRepository.GetAll()
                    .Where(o => o.OrderDate.Date == today)
                    .SumAsync(o => o.GrantTotal),
                TodayOrders = await _unitOfWork.OrderRepository.GetAll()
                    .Where(o => o.OrderDate.Date == today)
                    .CountAsync(),
                NewUsersToday = await _userManager.Users
                    .Where(u => u.CreatedDate.HasValue && u.CreatedDate.Value.Date == today)
                    .CountAsync(),
                OpenTickets = await _unitOfWork.TicketRepository
                    .GetAll()
                    .CountAsync(t => t.TicketStatus == TicketStatus.Open)
            };

            return stats;
        }

        public async Task<List<SalesDataDto>> GetSalesDataAsync(DateTime? startDate, DateTime? endDate)
        {
            var query = _unitOfWork.OrderRepository.GetAll();

            if (startDate.HasValue)
                query = query.Where(o => o.OrderDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(o => o.OrderDate <= endDate.Value);

            var salesData = await query
                .GroupBy(o => o.OrderDate.Date)
                .Select(g => new SalesDataDto
                {
                    Date = g.Key.ToString("yyyy-MM-dd"),
                    Revenue = g.Sum(o => o.GrantTotal),
                    OrderCount = g.Count()
                })
                .ToListAsync();

            return salesData;
        }

        public async Task<List<TopCategoryDto>> GetTopCategoriesAsync()
        {
            var topCategories = await _unitOfWork.CategoryRepository.GetAll()
                .Select(c => new TopCategoryDto
                {
                    CategoryName = c.CategoryName,
                    OrderCount = 0, // Placeholder - would need to join with orders
                    TotalRevenue = 0,
                    Percentage = 0
                })
                .Take(5)
                .ToListAsync();

            return topCategories;
        }

        public async Task<List<AdminOrderDto>> GetLatestOrdersAsync(int count)
        {
            var orders = await _unitOfWork.OrderRepository.GetAll()
                .Include(o => o.OrderStatues)
                .Include(o => o.PaymentType)
                .OrderByDescending(o => o.OrderDate)
                .Take(count)
                .Select(o => new AdminOrderDto
                {
                    Id = int.Parse(o.OrderId),
                    OrderNumber = o.OrderId,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.GrantTotal,
                    Status = o.OrderStatues.Name,
                    PaymentMethod = o.PaymentType.Name
                })
                .ToListAsync();

            return orders;
        }

        public async Task<UsersStatsDto> GetUsersStatsAsync()
        {
            var stats = new UsersStatsDto
            {
                TotalUsers = await _userManager.Users.Where(u => !u.IsDeleted).CountAsync(),
                ActiveUsers = await _userManager.Users.Where(u => !u.IsDeleted && (u.LockoutEnd == null || u.LockoutEnd < DateTimeOffset.UtcNow)).CountAsync(),
                SuspendedUsers = await _userManager.Users.Where(u => !u.IsDeleted && u.LockoutEnd > DateTimeOffset.UtcNow).CountAsync(),
                NewUsersThisMonth = await _userManager.Users
                    .Where(u => !u.IsDeleted
                        && u.CreatedDate.HasValue
                        && u.CreatedDate.Value.Month == DateTime.UtcNow.Month
                        && u.CreatedDate.Value.Year == DateTime.UtcNow.Year)
                    .CountAsync()
            };

            return stats;
        }

        #endregion
    }
}


