using Microsoft.AspNetCore.Mvc;

namespace CryptoTickerBot.API.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        [HttpGet]
        public IActionResult Error() => Problem();
    }
}
