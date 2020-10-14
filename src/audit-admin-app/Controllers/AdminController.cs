using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Telegram.Governor.Models;
using Telegram.Governor.Services;

namespace Covario.AuditAdminApp.Controllers
{
    [ApiController]
    [Route("/api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> _logger;
        private readonly ITelegramSession _telegramSession;

        public AdminController(
            ILogger<AdminController> logger,
            ITelegramSession telegramSession)
        {
            _logger = logger;
            _telegramSession = telegramSession;
        }

        [HttpGet]
        [HttpGet("State")]
        public string State()
        {
            return _telegramSession.State.ToString(); 
        }

        [HttpGet("Connect")]
        public Task<string> Connect(string governorAccountPhoneNumber = null, string responseCode = null,
            string mfaPassword = null)
        {
            _telegramSession.Connect(governorAccountPhoneNumber, responseCode, mfaPassword);
            return Task.FromResult(_telegramSession.State.ToString());
        }

        [HttpGet("Contacts")]
        public async Task<IEnumerable<TelegramContact>> GetContacts()
        {
            _logger.LogInformation("ConnectionId = {ConnectionId}", HttpContext.Connection.Id);
            var contacts = new List<TelegramContact>();
            await foreach (var contact in _telegramSession.GetContacts())
            {
                contacts.Add(contact);
            }

            return contacts;
        }

        [HttpGet("ContactForNumber")]
        public async Task<TelegramContact> GetContactForNumber(string phoneNumber)
        {
            _logger.LogInformation("ConnectionId = {ConnectionId}", HttpContext.Connection.Id);
            return await _telegramSession.GetContactForNumber(phoneNumber);
        }
    }
}
