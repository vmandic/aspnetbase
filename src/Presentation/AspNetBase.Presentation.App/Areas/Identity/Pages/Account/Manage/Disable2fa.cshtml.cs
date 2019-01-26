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
  public class Disable2faModel : PageModel
  {
    private readonly IManageAuthenticationService _manageAuthenticationService;

    public Disable2faModel(IManageAuthenticationService manageAuthenticationService)
    {
      _manageAuthenticationService = manageAuthenticationService;
    }

    [TempData]
    public string StatusMessage { get; set; }

    public async Task<IActionResult> OnGet()
    {
      var has2faEnabled = await _manageAuthenticationService.CheckUserHas2faEnabled(User);
      if (!has2faEnabled)
      {
        throw new InvalidOperationException($"Cannot disable 2FA for user with username '{User.Identity.Name}' as it's not currently enabled.");
      }

      return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
      var twoFaDisabled = await _manageAuthenticationService.Disable2fa(User);
      if (!twoFaDisabled)
      {
        throw new InvalidOperationException($"Unexpected error occurred disabling 2FA for user with username '{User.Identity.Name}'.");
      }

      StatusMessage = "2fa has been disabled. You can reenable 2fa when you setup an authenticator app";
      return RedirectToPage("./TwoFactorAuthentication");
    }
  }
}
