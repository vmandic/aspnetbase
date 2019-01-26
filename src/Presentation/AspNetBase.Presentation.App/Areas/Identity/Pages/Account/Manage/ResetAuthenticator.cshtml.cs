using System.Threading.Tasks;
using AspNetBase.Core.Contracts.Services.Identity.AccountManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetBase.Presentation.App.Areas.Identity.Pages.Account.Manage
{
    public class ResetAuthenticatorModel : PageModel
  {
    private readonly IManageAuthenticationService _manageAuthenticationService;

    public ResetAuthenticatorModel(IManageAuthenticationService manageAuthenticationService)
    {
      _manageAuthenticationService = manageAuthenticationService;
    }

    [TempData]
    public string StatusMessage { get; set; }

    public async Task<IActionResult> OnGet()
    {
      await _manageAuthenticationService.GetUserOrThrow(User);
      return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
      if (await _manageAuthenticationService.Reset2fa(User))
      {
        StatusMessage = "Your authenticator app key has been reset, you will need to configure your authenticator app using the new key.";
      }

      return RedirectToPage("./EnableAuthenticator");
    }
  }
}
