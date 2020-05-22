using Microsoft.AspNetCore.Mvc;

namespace LatinoNETOnline.TokenRefresher.Web.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]/[action]")]
    [ApiController]
    public class ResponsesController : ControllerBase
    {
        [HttpGet]
        public IActionResult Success([FromQuery]string message)
        {
            return Ok(message);
        }
        [HttpGet]
        public IActionResult Error([FromQuery]string message)
        {
            return Ok(message);
        }
    }
}
