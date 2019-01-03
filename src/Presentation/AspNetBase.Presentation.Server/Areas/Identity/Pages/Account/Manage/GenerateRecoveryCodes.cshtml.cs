using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetBase.Common.Utils.Extensions;
using AspNetBase.Core.Contracts.Services.Identity.AccountManagement;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace AspNetBase.Presentation.Server.Areas.Identity.Pages.Account.Manage
{
  public class GenerateRecoveryCodesModel : PageModel
  {
    private readonly IManageAuthenticationService _manageAuthenticationService;

    public GenerateRecoveryCodesModel(IManageAuthenticationService manageAuthenticationService)
    {
      _manageAuthenticationService = manageAuthenticationService;
    }

    [TempData]
    public string[] RecoveryCodes { get; set; }

    [TempData]
    public string StatusMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
      await CheckUserHas2faEnabled();
      return Page();
    }

    private async Task CheckUserHas2faEnabled(AppUser user = null)
    {
      if (!await _manageAuthenticationService.CheckUserHas2faEnabled(User, user))
      {
        throw new InvalidOperationException($"Cannot generate recovery codes for user with ID '{User.GetUserId()}' because they do not have 2FA enabled.");
      }
    }

    public async Task<IActionResult> OnPostAsync()
    {
      var user = await _manageAuthenticationService.GetUserOrThrow(User);
      await CheckUserHas2faEnabled(user);

      var recoveryCodes = await _manageAuthenticationService.GenerateNew2faRecoveryCodes(User, 10, user);

      RecoveryCodes = recoveryCodes.ToArray();
      StatusMessage = "You have generated new recovery codes.";
      return RedirectToPage("./ShowRecoveryCodes");
    }
  }
}
