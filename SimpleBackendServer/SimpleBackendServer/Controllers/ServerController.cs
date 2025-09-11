using Microsoft.AspNetCore.Mvc;

namespace SimpleBackendServer.Controllers
{
    [ApiController]
    [Route("/")]
    public class ServerController : ControllerBase
    {
        private static int _requestCount;

        private readonly ILogger<ServerController> _logger;

        public ServerController(ILogger<ServerController> logger)
        {
            _logger = logger;
        }

        [HttpGet("connect")]
        public IActionResult Connect()
        {
            var port = HttpContext.Connection.LocalPort;
            _logger.LogInformation($"Added connection to this server on port {port}");
            _requestCount++;
            return Ok($"Hello from server on port {port}. Total requests: {_requestCount}");
        }


        [HttpGet("status")]
        public IActionResult Status()
        {
            var port = HttpContext.Connection.LocalPort;
            _logger.LogInformation($"Reporting status of connections to this server on port {port}");
            return Ok(new { server = port, requests = _requestCount});
        }
    }
}
