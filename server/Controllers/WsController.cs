using Microsoft.AspNetCore.Mvc;
using server.Dtos;
using server.Handlers;

namespace server.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("web/ws")]
    public class WsController : ControllerBase
    {
        public WsController()
        { }

        [HttpPost("send")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public ActionResult Send(WsResponse response)
        {
            try
            {
                WsResponse msg = new()
                {
                    Message = response?.Message ?? DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff"),
                    Content = "Welcome!"
                };
                WebSocketHandler.Send(msg);
                return Ok();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}