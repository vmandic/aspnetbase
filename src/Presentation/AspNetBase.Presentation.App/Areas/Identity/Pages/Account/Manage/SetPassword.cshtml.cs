using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AspNetBase.Core.Contracts.Services.Identity.AccountManagement;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetBase.Presentation.App.Areas.Identity.Pages.Account.Manage
{
  public class SetPasswordModel : PageModel
  {
    private readonly IManageAccountService _manageAccountService;

    public SetPasswordModel(IManageAccountService manageAccountService)
    {
      _manageAccountService = manageAccountService;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    [TempData]
    public string StatusMessage { get; set; }

    public class InputModel
    {
      [Required]
      [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
      [DataType(DataType.Password)]
      [Display(Name = "New password")]
      public string NewPassword { get; set; }

      [DataType(DataType.Password)]
      [Display(Name = "Confirm new password")]
      [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
      public string ConfirmPassword { get; set; }
    }

    public async Task<IActionResult> OnGetAsync()
    {
      return (await _manageAccountService.CheckUserHasPassword(User)) ?
        RedirectToPage("./ChangePassword") :
        (IActionResult) Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
      if (!ModelState.IsValid)
      {
        return Page();
      }

      var result = await _manageAccountService.SetUserNewPassword(User, Input.NewPassword);

      if (!result.passwordSet)
      {
        foreach (var error in result.errors)
          ModelState.AddModelError(string.Empty, error);

        return Page();
      }

      StatusMessage = "Your password has been set.";

      return RedirectToPage();
    }
  }
}
