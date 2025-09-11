using Microsoft.AspNetCore.Mvc;

namespace LoadBalancer.Controllers
{
    [ApiController]
    [Route("/admin/")]
    public class RoutingConnectionController : ControllerBase
    {
        

        private readonly ILogger<RoutingConnectionController> _logger;
        private readonly HealthCheckerService _healthCheckerService;

        private readonly StatusReporterService _statusReporterService;

        public RoutingConnectionController(ILogger<RoutingConnectionController> logger, HealthCheckerService healthCheckerService, StatusReporterService statusReporterService)
        {
            _logger = logger;
            _healthCheckerService = healthCheckerService;
            _statusReporterService = statusReporterService;
        }

        [HttpGet("connect")]
        public IActionResult Connect()
        {
            // call method for get a healthy backend service
            var url = _healthCheckerService.GetNextHealthyBackend();

            if (url == string.Empty)
            {
                return NotFound("No url connection registered as healthy in Load Balancer");
            }

            return Ok($"connection established at : {url}");
        }

        [HttpGet("status")]
        public IActionResult Status()
        {
            var backendUrls = _healthCheckerService.GetAllHealthyBackends();

            // get healthy backend url calls

            return Ok();
        }
    }
}
