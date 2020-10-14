using System;
using System.Linq;
using System.Threading.Tasks;
using Covario.AuditAdminApp.Configuration;
using Covario.AuditAdminApp.Services;
using Covario.ChatApp.hub;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using TdLib;
using Telegram.Governor.Models;
using Telegram.Governor.Services;

namespace Covario.AuditAdminApp.Hubs
{
    [Authorize]
    public class AdminHub : Hub<IAdminHub>
    {
        private readonly ILogger<AdminHub> _logger;
        private readonly ITelegramSession _telegramSession;
        private readonly ITelegramService _telegramService;

        public AdminHub(
            ILogger<AdminHub> logger, 
            ITelegramSession telegramSession,
            ITelegramService telegramService)
        {
            _logger = logger;
            _telegramSession = telegramSession;
            _telegramService = telegramService;
        }
        
        public override Task OnConnectedAsync()
        {
            _logger.LogInformation("Hub connection from {user}", Context.User.Identity.Name);
            Clients.Caller.OnStateUpdated(_telegramSession.State);
            return base.OnConnectedAsync();
        }

        public async Task<TelegramSessionState> GetTelegramSessionState()
        {
            return _telegramSession.State;
        }

        public async Task Connect(string governorAccountPhoneNumber = null, string responseCode = null, string mfaPassword = null)
        {
            await _telegramSession.Connect(governorAccountPhoneNumber, responseCode, mfaPassword);
        }

        public async Task Disconnect()
        {
            _telegramService.ChatWatcherDisconnect(Context.ConnectionId);
            await _telegramSession.Disconnect();
        }
        
        public Task ChatWatch(long chatId)
        {
            _telegramService.ChatWatchSubscribe(Context.ConnectionId, chatId);
            return Task.CompletedTask;
        }

        public async Task CreateGroup(TelegramContact serviceContact, TelegramContact clientContact, string groupName)
        {   
            await _telegramSession.CreateGroup(serviceContact, clientContact, groupName);
        }

        public async Task CreateGroupByNumber(string serivePhoneNumber, string clientPhoneNumber, string groupName)
        {
            var serviceContact = await _telegramSession.GetContactForNumber(serivePhoneNumber);
            var clientContact = await _telegramSession.GetContactForNumber(clientPhoneNumber);
            await _telegramSession.CreateGroup(serviceContact, clientContact, groupName);
        }
    }
}
