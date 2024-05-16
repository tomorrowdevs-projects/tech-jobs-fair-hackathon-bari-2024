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
                WebSocketHandler.SendBroadcast(response);
                return Ok();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }


        [HttpPost("questions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public ActionResult Questions()
        {
            try
            {
                WebSocketHandler.PickQuestion();
                return Ok(WebSocketHandler.QuestionSelected);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}