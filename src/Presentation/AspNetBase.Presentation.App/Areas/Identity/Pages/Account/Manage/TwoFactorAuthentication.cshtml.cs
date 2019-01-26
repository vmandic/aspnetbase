using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetBase.Core.Contracts.Services.Identity.AccountManagement;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace AspNetBase.Presentation.App.Areas.Identity.Pages.Account.Manage
{
  public class TwoFactorAuthenticationModel : PageModel
  {
    private readonly IManageAuthenticationService _manageAuthenticationService;

    public TwoFactorAuthenticationModel(IManageAuthenticationService manageAuthenticationService)
    {
      _manageAuthenticationService = manageAuthenticationService;
    }

    public bool HasAuthenticator { get; set; }

    public int RecoveryCodesLeft { get; set; }

    [BindProperty]
    public bool Is2faEnabled { get; set; }

    public bool IsMachineRemembered { get; set; }

    [TempData]
    public string StatusMessage { get; set; }

    public async Task<IActionResult> OnGet()
    {
      (
        HasAuthenticator,
        Is2faEnabled,
        IsMachineRemembered,
        RecoveryCodesLeft
      ) = await _manageAuthenticationService.Get2faInfo(User);

      return Page();
    }

    public async Task<IActionResult> OnPost()
    {
      if (await _manageAuthenticationService.Forget2fa(User))
      {
        StatusMessage = "The current browser has been forgotten. When you login again from this browser you will be prompted for your 2fa code.";
      }
      return RedirectToPage();
    }
  }
}
