using AspNetBase.Core.App.Contracts.Services.Identity;
using AspNetBase.Core.App.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace AspNetBase.Presentation.App.Areas.Identity.Pages.Account
{
  [AllowAnonymous]
  public class ExternalLoginModel : ExternalLoginPageModel
  {
    private readonly IExternalSignInService _externalSignInService;

    public ExternalLoginModel(IExternalSignInService externalSignInService)
    {
      _externalSignInService = externalSignInService;
    }

    public IActionResult OnGetAsync()
    {
      return RedirectToPage("./Login");
    }

    public IActionResult OnPost(string provider, string returnUrl = null)
    {
      return _externalSignInService.ChallengeExternalLoginProvider(
        provider,
        Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl }));
    }

    public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
    {
      returnUrl = returnUrl ?? Url.Content("~/");

      var (result, externalLoginDto) =
        await _externalSignInService.SignInWithExternalProvider(remoteError);

      this.MapFromDto(externalLoginDto);

      if (result == SignInResult.Failed || result == SignInResult.LockedOut)
      {
        return RedirectToPage(Url.Page("./Login", new { ReturnUrl = returnUrl }));
      }
      else if (result == SignInResult.Success)
      {
        return LocalRedirect(returnUrl);
      }

      return Page();
    }

    public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
    {
      var (result, externalLoginDto, errorMessages) =
        await _externalSignInService.ConfirmExternalLogin(Input.Email);

      if (errorMessages.Count > 0)
        errorMessages.ForEach(msg => ModelState.AddModelError(string.Empty, msg));

      this.MapFromDto(externalLoginDto);

      if (!ModelState.IsValid || result != IdentityResult.Success)
        return Page();

      return LocalRedirect(returnUrl ?? Url.Content("~/"));
    }
  }
}
