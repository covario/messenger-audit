using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Covario.AuditAdminApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Telegram.Governor.Models;

namespace Covario.AuditAdminApp.Controllers
{
    [ApiController]
    [Route("/api/log")]
    public class LogController: ControllerBase
    {
        private readonly ILogger<LogController> _logger;
        private readonly IMessageAuditService _messageAuditService;

        public LogController(ILogger<LogController> logger, IMessageAuditService messageAuditService)
        {
            _logger = logger;
            _messageAuditService = messageAuditService;
        }

        [HttpGet("/api/log/{id}")]
        public async Task<IActionResult> DownloadAsync(string id)
        {
            if (_messageAuditService.LogExists(id))
            {
                var allLog = _messageAuditService.ReadLog(id).ToList();
                string json = System.Text.Json.JsonSerializer.Serialize(allLog);

                return Content(json, new MediaTypeHeaderValue("text/plain")) ;
            }
            
            return NotFound();
        }
    }
}
