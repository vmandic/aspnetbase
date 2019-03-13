using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetBase.Common.Utils.Extensions;
using AspNetBase.Core.App.Contracts.Services.Identity.AccountManagement;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetBase.Presentation.App.Areas.Identity.Pages.Account.Manage
{
  public class ExternalLoginsModel : PageModel
  {
    private readonly IManageAuthenticationService _manageAuthenticationService;

    public ExternalLoginsModel(IManageAuthenticationService manageAuthenticationService)
    {
      _manageAuthenticationService = manageAuthenticationService;
    }

    public IList<UserLoginInfo> CurrentLogins { get; set; }

    public IList<AuthenticationScheme> OtherLogins { get; set; }

    public bool ShowRemoveButton { get; set; }

    [TempData]
    public string StatusMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
      (CurrentLogins, OtherLogins, ShowRemoveButton) = await _manageAuthenticationService.GetUserLogins(User);

      return Page();
    }

    public async Task<IActionResult> OnPostRemoveLoginAsync(string loginProvider, string providerKey)
    {
      var loginRemoved = await _manageAuthenticationService.RemoveExternalLogin(
        User,
        loginProvider,
        providerKey);

      if (!loginRemoved)
      {
        throw new InvalidOperationException($"Unexpected error occurred removing external login for user with ID '{User.GetUserId()}'.");
      }

      StatusMessage = "The external login was removed.";
      return RedirectToPage();
    }

    public async Task<IActionResult> OnPostLinkLoginAsync(string provider)
    {
      return await _manageAuthenticationService.ChallengeExternalLogin(
        User,
        provider,
        Url.Page(
          "./ExternalLogins",
          pageHandler: "LinkLoginCallback"));
    }

    public async Task<IActionResult> OnGetLinkLoginCallbackAsync()
    {
      var result = await _manageAuthenticationService.LinkExternalLogin(User);

      if (!result)
      {
        throw new InvalidOperationException($"Unexpected error occurred adding external login for user with ID '{User.GetUserId()}'.");
      }

      StatusMessage = "The external login was added.";
      return RedirectToPage();
    }
  }
}
