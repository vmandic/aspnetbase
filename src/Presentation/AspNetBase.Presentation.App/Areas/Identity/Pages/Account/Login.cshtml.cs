using AspNetBase.Core.Contracts.Services.Identity;
using AspNetBase.Core.App.Models.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetBase.Presentation.App.Areas.Identity.Pages.Account
{
  [AllowAnonymous]
  public class LoginModel : PageModel
  {
    private readonly ISignInService _signInService;
    private readonly IExternalSignInService _externalSignInService;

    public LoginModel(
      IExternalSignInService externalSignInService,
      ISignInService signInService,
      IServiceProvider sp)
    {
      _signInService = signInService;
      _externalSignInService = externalSignInService;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public IList<AuthenticationScheme> ExternalLogins { get; set; }

    public string ReturnUrl { get; set; }

    [TempData]
    public string ErrorMessage { get; set; }

    public class InputModel
    {
      [Required]
      [EmailAddress]
      public string Email { get; set; }

      [Required]
      [DataType(DataType.Password)]
      public string Password { get; set; }

      [Display(Name = "Remember me?")]
      public bool RememberMe { get; set; }
    }

    public async Task OnGetAsync(string returnUrl = null)
    {
      if (!string.IsNullOrEmpty(ErrorMessage))
        ModelState.AddModelError(string.Empty, ErrorMessage);

      // Clear the existing external cookie to ensure a clean login process
      await _signInService.SignOut(IdentityConstants.ExternalScheme);

      ExternalLogins = (await _externalSignInService.GetExternalLoginProviders()).ToList();
      ReturnUrl = returnUrl ?? Url.Content("~/");
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
      if (ModelState.IsValid)
      {
        // This doesn't count login failures towards account lockout
        // To enable password failures to trigger account lockout, set lockoutOnFailure: true
        var (result, errorMessages) = await _signInService.SignInWithPassword(
          new LoginDto(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure : true));

        if (errorMessages.Count() > 0)
        {
          errorMessages.ForEach(msg => ModelState.AddModelError(string.Empty, msg));
          return Page();
        }
        else if (result.IsLockedOut)
        {
          return RedirectToPage("./Lockout");
        }

        returnUrl = returnUrl ?? Url.Content("~/");
        if (result.Succeeded)
        { 
          return LocalRedirect(returnUrl);
        }
        else if (result.RequiresTwoFactor)
        {
          return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, Input.RememberMe });
        }
      }

      // If we got this far, something failed, redisplay form
      return Page();
    }
  }
}
