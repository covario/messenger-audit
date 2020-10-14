using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Covario.AuditAdminApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Telegram.Governor.Services;

namespace Covario.AuditAdminApp.Pages.Admin
{
    public class AddGroupModel : PageModel
    {
        private readonly ITelegramService _telegramService;

        [BindProperty]
        public string Message { get; set; }
        
        [StringLength(60, MinimumLength = 3)]
        [Required]
        [BindProperty]
        public string SupportPhoneNumber { get; set; }
        
        [StringLength(60, MinimumLength = 3)]
        [Required]
        [BindProperty]
        public string ClientPhoneNumber { get; set; }
        
        [StringLength(60, MinimumLength = 3)]
        [Required]
        [BindProperty]
        public string GroupName { get; set; }

        public AddGroupModel(ITelegramService telegramService)
        {
            _telegramService = telegramService;
        }

        public void OnGet()
        {
            GroupName = "Covario - ";
        }


        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _telegramService.CreateGroupByNumber(SupportPhoneNumber, ClientPhoneNumber, GroupName);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return Page();
            }

            return RedirectToPage("ListGroups");
        }
    }
}
