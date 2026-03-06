using Microsoft.AspNetCore.Mvc;
using MongoConsumer.Models.DTOs.DeviceManager;
using MongoConsumer.Services.UAVChangeHandlers.Interfaces;

namespace MongoConsumer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceManagerWebhookController : ControllerBase
    {
        private readonly IUAVChangeHandlerFactory _handlerFactory;

        public DeviceManagerWebhookController(IUAVChangeHandlerFactory handlerFactory)
        {
            _handlerFactory = handlerFactory;
        }

        [HttpPost("uav-changed")]
        public async Task<ActionResult> UAVChanged(
            [FromBody] UAVChangedNotificationDto notification,
            CancellationToken cancellationToken
        )
        {
            IUAVChangeHandler handler = _handlerFactory.CreateHandler(notification.Operation);
            await handler.HandleAsync(notification.TailId, notification.NewTailId, cancellationToken);
            return Ok();
        }
    }
}
