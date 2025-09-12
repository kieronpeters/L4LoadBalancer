using Microsoft.AspNetCore.Mvc;

namespace LoadBalancer.Controllers
{
    [ApiController]
    [Route("/admin/")]
    public class RoutingConnectionController : ControllerBase
    {
        

        private readonly ILogger<RoutingConnectionController> _logger;
        private readonly IHealthCheckerService _healthCheckerService;
        private readonly IStatusReporterService _statusReporterService;

        private readonly IConnectionService _connectionService;

        public RoutingConnectionController(ILogger<RoutingConnectionController> logger, IHealthCheckerService healthCheckerService, IStatusReporterService statusReporterService, IConnectionService connectionService)
        {
            _logger = logger;
            _healthCheckerService = healthCheckerService;
            _statusReporterService = statusReporterService;
            _connectionService = connectionService;
        }

        [HttpGet("connect")]
        public async Task<IActionResult> Connect()
        {
            // call method for get a healthy backend service
            var url = _healthCheckerService.GetNextHealthyBackend();

            if (url == string.Empty)
            {
                return NotFound("No url connection registered as healthy in Load Balancer");
            }

            if (!await _connectionService.ConnectToServer(url)) {
                return NotFound("Server connection failure, unable to connect");
            }
        
            return Ok($"connection established at : {url}");
        }

        [HttpGet("status")]
        public async Task<IActionResult> Status()
        {
            var backendStatuses = await _statusReporterService.GetHealtyBackendStatuses();

            return Ok(backendStatuses);
        }
    }
}
