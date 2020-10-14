using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Covario.AuditAdminApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Telegram.Governor.Models;

namespace Covario.AuditAdminApp.Pages.Admin
{
    public class MessageLogModel : PageModel
    {
        private readonly ITelegramService _telegramService;
        private readonly IMessageAuditService _messageAuditService;

        public MessageLogModel(
            ITelegramService telegramService,
            IMessageAuditService messageAuditService)
        {
            _telegramService = telegramService;
            _messageAuditService = messageAuditService;
            Messages = new List<TelegramMessage>();
        }

        [BindProperty]
        //public List<(DateTime sent, string sender, string message)> Messages { get; set; }

        public long ChatId { get; private set; }

        public string Title { get; set; }

        public IEnumerable<TelegramMessage> Messages { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var hexId = ((string)RouteData.Values["chatId"]).Trim();
            if (hexId.Length == 16)
            {
                ChatId = Convert.ToInt64(hexId, 16);
                var chatGroup = await _telegramService.GetGroup(ChatId);
                Title = chatGroup.Title;

                var messages = new Dictionary<long, TelegramMessage>();

                // Load messages already logged.
                foreach (var messageFromAudit in _messageAuditService.ReadLog(ChatId))
                {
                    // Use Try Add to avoid errors if we managed to record the same message.
                    messages.TryAdd(messageFromAudit.MessageId, messageFromAudit);
                }
                
                // Build up a list of new messages to log as they should be logged in reverse time order.
                var toAudit = new List<TelegramMessage>();
                await foreach (var messageFromTelegram in _telegramService.GetMessageHistory(ChatId))
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

                Messages = messages.Values.OrderByDescending(m => m.Sent);
            }

            return Page();
        }
    }
}
