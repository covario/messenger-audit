using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Covario.AuditAdminApp.Controllers
{
    [ApiController]
    [Route("healthz")]
    public class HealthzController : ControllerBase
    {
        private readonly ILogger<HealthzController> _logger;

        public HealthzController(ILogger<HealthzController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var healthStatus = new
            {
                status = "Healthy",
                totalDuration = @"00:00:00.001",
                entries =
                    new
                    {
                        self = new
                        {
                            data = new { },
                            description = "trading-app",
                            duration = @"00:00:00.001",
                            status = "Healthy"
                        }
                    }
            };

            return Ok(healthStatus);
        }
    }
}
