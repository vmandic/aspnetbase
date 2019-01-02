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
using Microsoft.Extensions.Logging;

namespace AspNetBase.Presentation.Server.Areas.Identity.Pages.Account.Manage
{
  public class ChangePasswordModel : PageModel
  {
    private readonly IManageAccountService _manageAccountService;

    public ChangePasswordModel(IManageAccountService manageAccountService)
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
      [DataType(DataType.Password)]
      [Display(Name = "Current password")]
      public string OldPassword { get; set; }

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
      if (!await _manageAccountService.CheckUserHasPassword(User))
      {
        return RedirectToPage("./SetPassword");
      }

      return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
      if (!ModelState.IsValid)
      {
        return Page();
      }

      var passwordChanged = await _manageAccountService.ChangeUserPassword(
        User,
        Input.OldPassword,
        Input.NewPassword);

      if (passwordChanged)
      {
        StatusMessage = "Your password has been changed.";
      }

      return RedirectToPage();
    }
  }
}
