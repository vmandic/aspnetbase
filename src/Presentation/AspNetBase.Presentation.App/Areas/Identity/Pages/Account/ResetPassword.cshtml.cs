using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AspNetBase.Core.App.Contracts.Services.Identity;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetBase.Presentation.App.Areas.Identity.Pages.Account
{
  [AllowAnonymous]
  public class ResetPasswordModel : PageModel
  {
    private readonly IPasswordService _passwordService;

    public ResetPasswordModel(IPasswordService passwordService)
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

      [Required]
      [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
      [DataType(DataType.Password)]
      public string Password { get; set; }

      [DataType(DataType.Password)]
      [Display(Name = "Confirm password")]
      [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
      public string ConfirmPassword { get; set; }

      public string Code { get; set; }
    }

    public IActionResult OnGet(string code = null)
    {
      if (code == null)
      {
        return BadRequest("A code must be supplied for password reset.");
      }
      else
      {
        Input = new InputModel
        {
          Code = code
        };
        return Page();
      }
    }

    public async Task<IActionResult> OnPostAsync()
    {
      if (!ModelState.IsValid)
        return Page();

      var (_, errorMessages) =
      await _passwordService.ResetPassword(Input.Email, Input.Code, Input.Password);

      if (errorMessages.Count() > 0)
      {
        errorMessages.ForEach(msg => ModelState.AddModelError(string.Empty, msg));
        return Page();
      }

      // NOTE: do not disclose to user if the user exists
      return RedirectToPage("./ResetPasswordConfirmation");
    }
  }
}