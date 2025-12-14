using System.Threading.Tasks;
using Cartify.Application.Contracts.CustomerDtos;
using Cartify.Application.Services.Interfaces.Merchant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cartify.API.Controllers.MerchantControllers
{
    [Route("api/merchant/customers")]
    [ApiController]
    [Authorize(Roles = "Merchant")]
    public class CustomerController : ControllerBase
    {
        private readonly IMerchantCustomerServices _customerServices;

        public CustomerController(IMerchantCustomerServices customerServices)
        {
            _customerServices = customerServices;
        }

        // =========================================================
        // 🔹 GET CUSTOMER BY ID
        // =========================================================
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCustomerById([FromRoute] string userId)
        {
            var customer = await _customerServices.GetCustomerByIdAsync(userId);
            if (customer == null)
                return NotFound(new { message = "Customer not found ❌" });

            return Ok(customer);
        }

        // =========================================================
        // 🔹 GET CUSTOMERS BY STORE (Paged)
        // =========================================================
        [HttpGet("store/{storeId:int}")]
        public async Task<IActionResult> GetCustomersByStoreId(
            [FromRoute] int storeId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var customers = await _customerServices.GetCustomersByStoreIdAsync(storeId, page, pageSize);
            return Ok(customers);
        }

        // =========================================================
        // 🔹 GET TOTAL CUSTOMER COUNT BY STORE
        // =========================================================
        [HttpGet("store/{storeId:int}/count")]
        public async Task<IActionResult> GetCustomerCount([FromRoute] int storeId)
        {
            var count = await _customerServices.GetCustomerCountAsync(storeId);
            return Ok(new { storeId, totalCustomers = count });
        }
    }
}
