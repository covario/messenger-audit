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
    public class ListGroupsModel : PageModel
    {
        private readonly ITelegramService _telegramService;
        private IEnumerable<TelegramChat> _chats;

        public ListGroupsModel(ITelegramService telegramService)
        {
            _telegramService = telegramService;
        }

        public IEnumerable<TelegramChat> Chats => _chats;

        public async Task OnGet()
        {
            var chats = new List<TelegramChat>();
            await foreach (var chat in _telegramService.GetGroups())
            {
                chats.Add(chat);

            }

            _chats = chats;
        }
    }
}
