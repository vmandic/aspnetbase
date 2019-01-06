using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AspNetBase.Core.Contracts.Services.Identity.AccountManagement;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetBase.Presentation.Server.Areas.Identity.Pages.Account.Manage
{
  public partial class IndexModel : PageModel
  {
    private readonly IManageProfileService _manageProfileService;

    public IndexModel(
      IManageProfileService manageProfileService)
    {
      _manageProfileService = manageProfileService;
    }

    [BindProperty]
    public string Username { get; set; }

    public bool IsEmailConfirmed { get; set; }

    [TempData]
    public string StatusMessage { get; set; }

    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
      [Required]
      [EmailAddress]
      public string Email { get; set; }

      [Phone]
      [Display(Name = "Phone number")]
      public string PhoneNumber { get; set; }
    }

    public async Task<IActionResult> OnGetAsync()
    {
      var profile = await _manageProfileService.GetUserProfile(User);

      Username = profile.UserName;

      Input = new InputModel
      {
        Email = profile.Email,
        PhoneNumber = profile.PhoneNumber
      };

      IsEmailConfirmed = profile.IsEmailConfirmed;

      return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
      if (!ModelState.IsValid)
        return Page();

      var result = await _manageProfileService.SaveUserProfile(User, Input.Email, Input.PhoneNumber);

      if (result.saved)
      {
        StatusMessage = "Your profile has been updated";
        return RedirectToPage();
      }

      foreach (var error in result.errors)
        ModelState.AddModelError(string.Empty, error);

      return Page();
    }

    public async Task<IActionResult> OnPostSendVerificationEmailAsync()
    {
      if (!ModelState.IsValid)
        return Page();

      await _manageProfileService.SendVerificationEmail(
        User,
        (string code, int userId) =>
        Url.Page(
          "/Account/ConfirmEmail",
          pageHandler : null,
          values : new { userId, code },
          protocol : Request.Scheme));

      StatusMessage = "Verification email sent. Please check your email.";
      return RedirectToPage();
    }
  }
}
