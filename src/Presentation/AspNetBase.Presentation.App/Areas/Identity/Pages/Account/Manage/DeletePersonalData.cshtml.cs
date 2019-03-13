using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AspNetBase.Core.App.Contracts.Services.Identity.AccountManagement;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace AspNetBase.Presentation.App.Areas.Identity.Pages.Account.Manage
{
  public class DeletePersonalDataModel : PageModel
  {
    private readonly IManageAccountService _manageAccountService;

    public DeletePersonalDataModel(
      IManageAccountService manageAccountService)
    {
      _manageAccountService = manageAccountService;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
      [Required]
      [DataType(DataType.Password)]
      public string Password { get; set; }
    }

    public bool RequirePassword { get; set; }

    public async Task<IActionResult> OnGet()
    {
      RequirePassword = await _manageAccountService.CheckUserHasPassword(User);
      return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
      var user = await _manageAccountService.GetUserOrThrow(User);
      RequirePassword = await _manageAccountService.CheckUserHasPassword(User, user);

      if (RequirePassword)
      {
        if (!await _manageAccountService.CheckUserPassword(User, Input.Password, user))
        {
          ModelState.AddModelError(string.Empty, "Password not correct.");
          return Page();
        }
      }

      var result = await _manageAccountService.DeleteAccount(User, user);
      if (!result)
      {
        throw new InvalidOperationException($"Unexpected error occurred deleteing user with ID '{user.Id}'.");
      }

      return Redirect("~/");
    }
  }
}
