using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HealthCheck.MicroB.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MicroBController : ControllerBase
    {

        private readonly ILogger<MicroBController> _logger;

        public MicroBController(ILogger<MicroBController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
