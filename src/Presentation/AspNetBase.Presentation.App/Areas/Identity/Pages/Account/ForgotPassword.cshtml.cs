using AspNetBase.Core.App.Contracts.Services.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace AspNetBase.Presentation.App.Areas.Identity.Pages.Account
{
  [AllowAnonymous]
  public class ForgotPasswordModel : PageModel
  {
    private readonly IPasswordService _passwordService;

    public ForgotPasswordModel(IPasswordService passwordService)
    {
      this._passwordService = passwordService;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
      [Required]
      [EmailAddress]
      public string Email { get; set; }
    }

    public async Task<IActionResult> OnPostAsync()
    {
      if (ModelState.IsValid)
      {
        await _passwordService.ForgotPassword(
          Input.Email,
          (resetToken) => Url.Page(
            "/Account/ResetPassword",
            pageHandler: null,
            values: new { resetToken },
            protocol: "https"));

        // Don't reveal that the user does not exist or is not confirmed
        RedirectToPage("./ForgotPasswordConfirmation");
      }

      return Page();
    }
  }
}
