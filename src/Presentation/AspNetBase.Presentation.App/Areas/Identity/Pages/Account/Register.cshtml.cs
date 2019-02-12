using AspNetBase.Core.App.Models.Identity;
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
    public RegisterAccountFm Input { get; set; }

    public string ReturnUrl { get; set; }

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
