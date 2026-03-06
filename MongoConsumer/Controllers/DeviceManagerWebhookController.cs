using Microsoft.AspNetCore.Mvc;
using MongoConsumer.Models.DTOs.DeviceManager;

namespace MongoConsumer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceManagerWebhookController : ControllerBase
    {
        [HttpPost("uav-changed")]
        public async Task<ActionResult> UAVChanged(
            [FromBody] UAVChangedNotificationDto notification,
            CancellationToken cancellationToken
        )
        {
            // TODO: handle UAV change based on notification.Operation
            return Ok();
        }
    }
}
