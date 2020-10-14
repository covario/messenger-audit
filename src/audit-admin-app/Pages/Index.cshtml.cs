using System.Threading.Tasks;
using Covario.AuditAdminApp.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Covario.AuditAdminApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            // Handle login flow
            if (! User.Identity.IsAuthenticated)
            {

            }
        }

        public async Task<IActionResult> OnGetLoginAsync()
        {
            return RedirectToPage("/admin/index");
            //return Redirect("https://www.bbc.co.uk");
        }
        
        public async Task<IActionResult> OnGetLogoutAsync()
        {
            return SignOut("oidc", "Cookies");
        }
    }
}