using Cartify.API.Controllers.MerchantControllers;
using Cartify.Application.Contracts.Admin;
using Cartify.Application.Contracts.OrderDtos;
using Cartify.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cartify.API.Controllers
{
    /// <summary>
    /// Admin Controller - Handles all admin-specific operations
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment _env;

        public AdminController(IAdminService adminService, Microsoft.AspNetCore.Hosting.IWebHostEnvironment env)
        {
            _adminService = adminService;
            _env = env;
        }

        #region User Management

        /// <summary>
        /// Get all users with pagination
        /// </summary>
        [HttpGet("Users")]
        public async Task<IActionResult> GetAllUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var users = await _adminService.GetAllUsersAsync(page, pageSize);
            return Ok(users);
        }

        /// <summary>
        /// Get user by ID
        /// </summary>
        [HttpGet("Users/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _adminService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        /// <summary>
        /// Create new user
        /// </summary>
        [HttpPost("Users")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
        {
            var result = await _adminService.CreateUserAsync(dto);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result);
        }

        /// <summary>
        /// Update user
        /// </summary>
        [HttpPut("Users/{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserDto dto)
        {
            var result = await _adminService.UpdateUserAsync(id, dto);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result);
        }

        /// <summary>
        /// Delete user (soft delete)
        /// </summary>
        [HttpDelete("Users/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _adminService.DeleteUserAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result);
        }

        /// <summary>
        /// Suspend user account
        /// </summary>
        [HttpPut("Users/{id}/Suspend")]
        public async Task<IActionResult> SuspendUser(string id)
        {
            var result = await _adminService.SuspendUserAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result);
        }

        /// <summary>
        /// Activate user account
        /// </summary>
        [HttpPut("Users/{id}/Activate")]
        public async Task<IActionResult> ActivateUser(string id)
        {
            var result = await _adminService.ActivateUserAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result);
        }

        #endregion

        #region Store Management

        /// <summary>
        /// Get all stores
        /// </summary>
        [HttpGet("Stores")]
        public async Task<IActionResult> GetAllStores([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var stores = await _adminService.GetAllStoresAsync(page, pageSize);
            return Ok(stores);
        }

        /// <summary>
        /// Get store by ID
        /// </summary>
        [HttpGet("Stores/{id}")]
        public async Task<IActionResult> GetStoreById(int id)
        {
            var store = await _adminService.GetStoreByIdAsync(id);
            if (store == null)
                return NotFound();
            return Ok(store);
        }

        /// <summary>
        /// Create new store
        /// </summary>
        [HttpPost("Stores")]
        public async Task<IActionResult> CreateStore([FromBody] CreateStoreDto dto)
        {
            var result = await _adminService.CreateStoreAsync(dto);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result);
        }

        /// <summary>
        /// Update store
        /// </summary>
        [HttpPut("Stores/{id}")]
        public async Task<IActionResult> UpdateStore(int id, [FromBody] UpdateStoreDto dto)
        {
            var result = await _adminService.UpdateStoreAsync(id, dto);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result);
        }

        /// <summary>
        /// Delete store
        /// </summary>
        [HttpDelete("Stores/{id}")]
        public async Task<IActionResult> DeleteStore(int id)
        {
            var result = await _adminService.DeleteStoreAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result);
        }

        #endregion

        #region Order Management

        /// <summary>
        /// Get all orders
        /// </summary>
        [HttpGet("Orders")]
        public async Task<IActionResult> GetAllOrders([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var orders = await _adminService.GetAllOrdersAsync(page, pageSize);
            return Ok(orders);
        }

        /// <summary>
        /// Get order by ID
        /// </summary>
        [HttpGet("Orders/{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _adminService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();
            return Ok(order);
        }

        /// <summary>
        /// Update order status
        /// </summary>
        [HttpPut("Orders/{id}/Status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusDto dto)
        {
            var result = await _adminService.UpdateOrderStatusAsync(id, dto);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result);
        }

        /// <summary>
        /// Cancel order
        /// </summary>
        [HttpDelete("Orders/{id}")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var result = await _adminService.CancelOrderAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result);
        }

        #endregion

        #region Analytics & Reports

        /// <summary>
        /// Get dashboard statistics
        /// </summary>
        [HttpGet("Analytics/Dashboard")]
        public async Task<IActionResult> GetDashboardStats()
        {
            var stats = await _adminService.GetDashboardStatsAsync();
            return Ok(stats);
        }

        /// <summary>
        /// Get sales data for charts
        /// </summary>
        [HttpGet("Analytics/Sales")]
        public async Task<IActionResult> GetSalesData([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var salesData = await _adminService.GetSalesDataAsync(startDate, endDate);
            return Ok(salesData);
        }

        /// <summary>
        /// Get top categories
        /// </summary>
        [HttpGet("Analytics/TopCategories")]
        public async Task<IActionResult> GetTopCategories()
        {
            var categories = await _adminService.GetTopCategoriesAsync();
            return Ok(categories);
        }

        /// <summary>
        /// Get latest orders
        /// </summary>
        [HttpGet("Analytics/LatestOrders")]
        public async Task<IActionResult> GetLatestOrders([FromQuery] int count = 10)
        {
            var orders = await _adminService.GetLatestOrdersAsync(count);
            return Ok(orders);
        }

        /// <summary>
        /// Get users statistics
        /// </summary>
        [HttpGet("Analytics/Users")]
        public async Task<IActionResult> GetUsersStats()
        {
            var stats = await _adminService.GetUsersStatsAsync();
            return Ok(stats);
        }

        /// <summary>
        /// Development-only diagnostics endpoint that returns counts and sample data for dashboard, users, stores, categories and sales.
        /// Visible only when ASPNETCORE_ENVIRONMENT=Development. Marked AllowAnonymous for easier local testing but will return 404 outside Development.
        /// </summary>
        [HttpGet("Diagnostics/Counts")]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public async Task<IActionResult> GetDiagnostics()
        {
            if (!_env.IsDevelopment())
            {
                return NotFound();
            }

            var dashboard = await _adminService.GetDashboardStatsAsync();
            var usersStats = await _adminService.GetUsersStatsAsync();
            var sales = await _adminService.GetSalesDataAsync(null, null);
            var topCategories = await _adminService.GetTopCategoriesAsync();
            var latestOrders = await _adminService.GetLatestOrdersAsync(10);

            return Ok(new
            {
                Dashboard = dashboard,
                UsersStats = usersStats,
                SalesSample = sales.OrderByDescending(s => s.Date).Take(30),
                TopCategories = topCategories,
                LatestOrders = latestOrders
            });
        }

        #endregion
    }
}


