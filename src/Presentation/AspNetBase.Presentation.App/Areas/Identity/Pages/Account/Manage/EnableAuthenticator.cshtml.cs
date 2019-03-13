using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AspNetBase.Core.App.Contracts.Services.Identity.AccountManagement;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetBase.Presentation.App.Areas.Identity.Pages.Account.Manage
{
    public class EnableAuthenticatorModel : PageModel
  {
    private readonly IManageAuthenticationService _manageAuthenticationService;
    private readonly UrlEncoder _urlEncoder;

    private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

    public EnableAuthenticatorModel(
      IManageAuthenticationService manageAuthenticationService,
      UrlEncoder urlEncoder)
    {
      _manageAuthenticationService = manageAuthenticationService;
      _urlEncoder = urlEncoder;
    }

    public string SharedKey { get; set; }

    public string AuthenticatorUri { get; set; }

    [TempData]
    public string[] RecoveryCodes { get; set; }

    [TempData]
    public string StatusMessage { get; set; }

    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
      [Required]
      [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
      [DataType(DataType.Text)]
      [Display(Name = "Verification Code")]
      public string Code { get; set; }
    }

    public async Task<IActionResult> OnGetAsync()
    {
      await Load2faSharedKeyAndAuthenticatorUri();
      return Page();
    }

    private async Task Load2faSharedKeyAndAuthenticatorUri(AppUser user = null)
    {
      var (sharedKey, qrCodeUri) = await _manageAuthenticationService.Get2faSharedKeyAndQrCodeUri(User, user);

      SharedKey = sharedKey;
      AuthenticatorUri = qrCodeUri;
    }

    public async Task<IActionResult> OnPostAsync()
    {
      var user = await _manageAuthenticationService.GetUserOrThrow(User);

      if (!ModelState.IsValid)
      {
        await Load2faSharedKeyAndAuthenticatorUri(user);
        return Page();
      }

      var result = await _manageAuthenticationService.Enable2fa(User, Input.Code, user);

      if (!result.isTokenValid)
      {
        ModelState.AddModelError("Input.Code", "Verification code is invalid.");
        await Load2faSharedKeyAndAuthenticatorUri(user);
        return Page();
      }

      StatusMessage = "Your authenticator app has been verified.";

      if (result.recoveryCodes.Count() > 0)
      {
        RecoveryCodes = result.recoveryCodes.ToArray();
        return RedirectToPage("./ShowRecoveryCodes");
      }

      return RedirectToPage("./TwoFactorAuthentication");
    }
  }
}
