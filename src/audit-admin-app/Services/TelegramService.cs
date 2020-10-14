using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Covario.AuditAdminApp.Hubs;
using Covario.ChatApp.hub;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Telegram.Governor.Models;
using Telegram.Governor.Services;

namespace Covario.AuditAdminApp.Services
{
    public class TelegramService : IDisposable, ITelegramService
    {
        private readonly ILogger<TelegramService> _logger;
        private ITelegramSession _telegramSession;
        private readonly IHubContext<AdminHub, IAdminHub> _hub;
        private readonly IMessageAuditService _messageAuditService;
        private readonly ConcurrentDictionary<string, List<long>> _chatSubscriptions;
        private IDisposable _stateChangeSub;
        private IDisposable _messageSub;
        private IDisposable _chatSub;

        public TelegramService(
            ILogger<TelegramService> logger,
            ITelegramSession telegramSession, 
            IHubContext<AdminHub, IAdminHub> hub,
            IMessageAuditService messageAuditService)
        {
            _logger = logger;
            _telegramSession = telegramSession;
            _hub = hub;
            _messageAuditService = messageAuditService;
            _chatSubscriptions = new ConcurrentDictionary<string, List<long>>();
            _stateChangeSub = _telegramSession.StateChange.Subscribe(OnStateChanged);
            _messageSub = _telegramSession.MessageFeed.Subscribe(NewMessage);
            _chatSub = _telegramSession.ChatFeed
                .ObserveOn(NewThreadScheduler.Default)
                .Subscribe(
                    async chat => await SyncLog(chat.ChatId)
                );
        }

        public void Start()
        {
            _telegramSession.StartSession();
        }

        public void Disconnect()
        {
            _telegramSession.Disconnect();
        }

        public void ChatWatchSubscribe(string connectionId, long chatId)
        {
            var subs = _chatSubscriptions.GetOrAdd(connectionId, _ => new List<long>());
            subs.Add(chatId);
        }

        public void ChatWatcherDisconnect(string connectionId)
        {
            var subs = _chatSubscriptions.GetOrAdd(connectionId, _ => new List<long>());
            subs.Clear();
        }


        private void NewChat(TelegramChat chat)
        {
            SyncLog(chat.ChatId).RunSynchronously();
        }

        public async Task SyncLog(long chatId)
        {
            // Slow down syncing logs for 10 seconds

            _logger.LogInformation("Waiting to Reconcile Log for {chatId}", chatId);
            Thread.Sleep(10000);
            _logger.LogInformation("Reconciling Log for {chatId}", chatId);

            var messages = new Dictionary<long, TelegramMessage>();

            // Load messages already logged.
            foreach (var messageFromAudit in _messageAuditService.ReadLog(chatId))
            {
                // Use Try Add to avoid errors if we managed to record the same message.
                messages.TryAdd(messageFromAudit.MessageId, messageFromAudit);
            }

            // Build up a list of new messages to log as they should be logged in reverse time order.
            var toAudit = new List<TelegramMessage>();
            await foreach (var messageFromTelegram in GetMessageHistory(chatId))
            {
                // Check if we already audited this message
                if (!messages.ContainsKey(messageFromTelegram.MessageId))
                {
                    // The message is new so we add it to the output and plan to log it
                    messages.Add(messageFromTelegram.MessageId, messageFromTelegram);
                    toAudit.Add(messageFromTelegram);
                }
            }

            if (toAudit.Any())
            {
                _messageAuditService.LogMessage(toAudit.OrderBy(m => m.Sent).ToList());
            }
            _logger.LogInformation("Reconciled Log for {chatId}", chatId);
        }

        private void OnStateChanged(TelegramSessionState state)
        {
            _hub.Clients.All.OnStateUpdated(state);
        }

        public TelegramSessionState Connect(string governorAccountPhoneNumber = null, string responseCode = null,
            string mfaPassword = null)
        {
            _telegramSession.Connect(governorAccountPhoneNumber, responseCode, mfaPassword);
            return _telegramSession.State;
        }

        public void NewMessage(TelegramMessage message)
        {
            _logger.LogInformation(
                "{ChatName} {Sender} {SentDateTime} {Message}",
                message.ChatName,
                message.SenderContact.PhoneNumber,
                message.Sent,
                message.Text);
            
            _messageAuditService.LogMessage(new List<TelegramMessage>(){message});

            var chatId = message.ChatId;
            foreach (var sub in _chatSubscriptions)
            {
                var conn = _hub.Clients.Client(sub.Key);
                if (conn != null && sub.Value.Contains(chatId)) 
                    conn.OnNewMessage(message);
            }
        }

        public async Task CreateGroupByNumber(string supportPhoneNumber, string clientPhoneNumber, string groupName)
        {
            var supportContact = await _telegramSession.GetContactForNumber(supportPhoneNumber);
            if (supportContact == null)
                throw new TelegramCreateGroupException("Invalid Support Contact");

            var clientContact = await _telegramSession.GetContactForNumber(clientPhoneNumber);
            if (clientContact == null)
                throw new TelegramCreateGroupException("Invalid Client Contact");

            if (clientContact == supportContact)
            {
                throw new TelegramCreateGroupException("Client and Support Contacts must be different");
            }

            await _telegramSession.CreateGroup(supportContact, clientContact, groupName);
        }

        public TelegramSessionState GetTelegramSessionState()
        {
            return _telegramSession.State;
        }

        public async IAsyncEnumerable<TelegramChat> GetGroups()
        {
            await foreach (var chat in _telegramSession.GetChats())
            {
                yield return chat;
            }
        }

        public async Task<TelegramChat> GetGroup(long chatId)
        {
            return await _telegramSession.GetGroup(chatId);
        }

        public async IAsyncEnumerable<TelegramContact> GetContacts()
        {
            await foreach (var contact in _telegramSession.GetContacts())
            {
                yield return contact;
            }
        }

        public async IAsyncEnumerable<TelegramMessage> GetMessageHistory(long chatId)
        {
            await foreach (var message in _telegramSession.GetMessageHistory(chatId))
            {
                yield return message;
            }
        }

        public void Dispose()
        {
            _telegramSession?.Dispose();
            _telegramSession = null;
            _stateChangeSub?.Dispose();
            _stateChangeSub = null;
            _messageSub?.Dispose();
            _messageSub = null;
            _chatSub?.Dispose();
            _chatSub = null;
        }
    }
}
