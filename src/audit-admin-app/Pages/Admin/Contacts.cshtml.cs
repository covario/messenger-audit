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
    public class ContactsModel : PageModel
    {
        private readonly ITelegramService _telegramService;
        private IEnumerable<TelegramContact> _contacts;

        public ContactsModel(ITelegramService telegramService)
        {
            _telegramService = telegramService;
        }
        public IEnumerable<TelegramContact> Contacts => _contacts;

        public async Task OnGet()
        {
            var contacts = new List<TelegramContact>();
            await foreach (var contact in _telegramService.GetContacts())
            {
                contacts.Add(contact);
                
            }

            _contacts = contacts;
        }
    }
}
