using AspNetBase.Core.Contracts.Services.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace AspNetBase.Presentation.Server.Areas.Identity.Pages.Account
{
  [AllowAnonymous]
  public class LoginWith2faModel : PageModel
  {
    private readonly ITwoFactorAuthService _2faService;

    public LoginWith2faModel(ITwoFactorAuthService twoFactorAuthService)
    {
      _2faService = twoFactorAuthService;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public bool RememberMe { get; set; }

    public string ReturnUrl { get; set; }

    public class InputModel
    {
      [Required]
      [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
      [DataType(DataType.Text)]
      [Display(Name = "Authenticator code")]
      public string TwoFactorCode { get; set; }

      [Display(Name = "Remember this machine")]
      public bool RememberMachine { get; set; }
    }

    public async Task<IActionResult> OnGetAsync(bool rememberMe, string returnUrl = null)
    {
      // Ensure the user has gone through the username & password screen first
      await _2faService.EnsureUserForTwoFactorAuthentication();

      ReturnUrl = returnUrl;
      RememberMe = rememberMe;

      return Page();
    }

    public async Task<IActionResult> OnPostAsync(bool rememberMe, string returnUrl = null)
    {
      if (!ModelState.IsValid)
        return Page();

      var authenticatorCode = Input.TwoFactorCode
        .Replace(" ", string.Empty)
        .Replace("-", string.Empty);

      var result = await _2faService.SignIn(
        authenticatorCode,
        rememberMe,
        Input.RememberMachine);

      if (result.Succeeded)
      {
        returnUrl = returnUrl ?? Url.Content("~/");
        return LocalRedirect(returnUrl);
      }
      else if (result.IsLockedOut)
      {
        return RedirectToPage("./Lockout");
      }
      else
      {
        ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
        return Page();
      }
    }
  }
}
