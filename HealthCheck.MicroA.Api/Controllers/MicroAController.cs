using HealthCheck.MicroA.Api.Domain;
using HealthCheck.MicroA.Api.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace HealthCheck.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MicroAController : ControllerBase
    {
        private readonly TripwireHealthCheck _yripwireHealthCheck;
        private readonly ForcedHealthCheck _forcedHealthCheck;
        private readonly ILogger<MicroAController> _logger;

        public MicroAController(
                                TripwireHealthCheck yripwireHealthCheck,
                                ForcedHealthCheck forcedHealthCheck,
                                ILogger<MicroAController> logger)
        {
            _yripwireHealthCheck = yripwireHealthCheck;
            _forcedHealthCheck = forcedHealthCheck;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<int> Get()
        {
            _logger.LogInformation("GETTTTTT");
            return _yripwireHealthCheck.Trip();
        }

        [HttpPost("force")]
        public void PostForce([FromQuery] string status)
        {
            _forcedHealthCheck.Force(status, Environment.MachineName);
        }

        [HttpPost]
        public IActionResult Post(Drone drone)
        {
            _logger.LogInformation("Cadastro do drone {0} de Id: {1} ", drone.Name, drone.Id);
            return Ok();
        }
    }
}
