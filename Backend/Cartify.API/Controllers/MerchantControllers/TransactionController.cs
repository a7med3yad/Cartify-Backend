using Cartify.Application.Services.Interfaces.Merchant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cartify.API.Controllers.MerchantControllers
{
    [Route("api/merchant/transactions")]
    [ApiController]
    [Authorize(Roles = "Merchant")]
    public class TransactionController : ControllerBase
    {
        private readonly IMerchantTransactionServices _transactionServices;
        public TransactionController(IMerchantTransactionServices _transactionServices)
        {
            this._transactionServices = _transactionServices;
        }

        // =========================================================
        // 🔹 GET TRANSACTIONS BY STORE ID (Paged)
        // =========================================================
        [HttpGet("store/{storeId:int}")]
        public async Task<IActionResult> GetTransactionsByStoreId(
            [FromRoute] int storeId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (storeId <= 0)
                return BadRequest(new { message = "Invalid store ID" });

            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var transactions = await _transactionServices.GetTransactionsByStoreIdAsync(storeId, page, pageSize);
            return Ok(transactions);
        }

        // =========================================================
        // 🔹 GET TRANSACTION SUMMARY
        // =========================================================
        [HttpGet("store/{storeId:int}/summary")]
        public async Task<IActionResult> GetTransactionSummary(
            [FromRoute] int storeId,
            [FromQuery] string period = "monthly")
        {
            if (storeId <= 0)
                return BadRequest(new { message = "Invalid store ID" });

            var summary = await _transactionServices.GetTransactionSummaryAsync(storeId, period);
            return Ok(summary);
        }
    }
}
