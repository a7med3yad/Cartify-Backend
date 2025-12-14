using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cartify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        [HttpGet("GetNotifications/{userId}")]
        public async Task<IActionResult> GetNotifications(Guid userId)
        {
            await Task.CompletedTask;
            return Ok();
        }

        [HttpGet("GetNotification/{id}")]
        public async Task<IActionResult> GetNotification(Guid id)
        {
            await Task.CompletedTask;
            return Ok();
        }

        [HttpPost("MarkRead/{id}")]
        public async Task<IActionResult> MarkRead(Guid id)
        {
            await Task.CompletedTask;
            return Ok();
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await Task.CompletedTask;
            return Ok();
        }
    }
}
