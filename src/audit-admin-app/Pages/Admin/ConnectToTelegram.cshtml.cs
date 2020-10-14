using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Covario.AuditAdminApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Telegram.Governor.Models;

namespace Covario.AuditAdminApp.Pages.Admin
{
    public class ConnectToTelegramModel : PageModel
    {
        private readonly ITelegramService _telegramService;

        public ConnectToTelegramModel(ITelegramService telegramService)
        {
            _telegramService = telegramService;
        }

        public TelegramSessionState SessionState => _telegramService?.GetTelegramSessionState() ?? TelegramSessionState.Uninitialized;

        public string SessionStateName => SessionState.ToString();

        [BindProperty]
        public string ServiceAccount { get; set; }
        
        [BindProperty] 
        public string ResponseCode { get; set; }
        
        [BindProperty, DataType(DataType.Password)]
        public string Password { get; set; }
        
        public async Task<IActionResult> OnPost()
        {
            _telegramService.Connect(ServiceAccount, ResponseCode, Password);
            return Page();
        }

        public async Task<IActionResult> OnPostDisconnectAsync()
        {
            _telegramService.Disconnect();
            return RedirectToPage();
        }
    }
}
