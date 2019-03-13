using AspNetBase.Core.App.Contracts.Services.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace AspNetBase.Presentation.App.Areas.Identity.Pages.Account
{
  [AllowAnonymous]
  public class LoginWithRecoveryCodeModel : PageModel
  {
    private readonly ISignInService _signInService;
    private readonly ITwoFactorAuthService _2faService;

    public LoginWithRecoveryCodeModel(ISignInService signInService, ITwoFactorAuthService twoFactorAuthService)
    {
      this._signInService = signInService;
      this._2faService = twoFactorAuthService;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public string ReturnUrl { get; set; }

    public class InputModel
    {
      [BindProperty]
      [Required]
      [DataType(DataType.Text)]
      [Display(Name = "Recovery Code")]
      public string RecoveryCode { get; set; }
    }

    public async Task<IActionResult> OnGetAsync(string returnUrl = null)
    {
      // Ensure the user has gone through the username & password screen first
      await _2faService.EnsureUserForTwoFactorAuthentication();

      ReturnUrl = returnUrl;

      return Page();
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
      if (!ModelState.IsValid)
        return Page();

      var recoveryCode = Input.RecoveryCode.Replace(" ", string.Empty);
      var result = await _2faService.RecoveryCodeSignInAsync(recoveryCode);

      if (result.Succeeded)
      {
        return LocalRedirect(returnUrl ?? Url.Content("~/"));
      }
      else if (result.IsLockedOut)
      {
        return RedirectToPage("./Lockout");
      }
      else
      {
        ModelState.AddModelError(string.Empty, "Invalid recovery code entered.");
        return Page();
      }
    }
  }
}
