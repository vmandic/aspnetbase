using AspNetBase.Core.Contracts.Services.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetBase.Presentation.App.Areas.Identity.Pages.Account
{
  [AllowAnonymous]
  public class RegisterModel : PageModel
  {
    private readonly IRegisterAccountService _registerAccountService;

    public RegisterModel(
        IRegisterAccountService registerAccountService)
    {
      this._registerAccountService = registerAccountService;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public string ReturnUrl { get; set; }

    public class InputModel
    {
      [Required]
      [EmailAddress]
      [Display(Name = "Email")]
      public string Email { get; set; }

      [Required]
      [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
      [DataType(DataType.Password)]
      [Display(Name = "Password")]
      public string Password { get; set; }

      [DataType(DataType.Password)]
      [Display(Name = "Confirm password")]
      [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
      public string ConfirmPassword { get; set; }
    }

    public void OnGet(string returnUrl = null)
    {
      ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
      if (ModelState.IsValid)
      {
        var (accountCreated, _, errorMessages) =
          await _registerAccountService.CreateAccountAndSignIn(
            Input.Email,
            Input.Password,
            (code, userId) => Url.Page(
              "/Account/ConfirmEmail",
              pageHandler: null,
              values: new { userId, code },
              protocol: "https"));

        if (accountCreated)
          return LocalRedirect(returnUrl ?? Url.Content("~/"));

        errorMessages.ForEach(msg => ModelState.AddModelError(string.Empty, msg));
      }

      // If we got this far, something failed, redisplay form
      return Page();
    }
  }
}
