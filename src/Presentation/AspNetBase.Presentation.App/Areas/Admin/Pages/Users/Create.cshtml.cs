using System.Linq;
using System.Threading.Tasks;
using AspNetBase.Core.App.Models.Identity;
using AspNetBase.Core.Contracts.Services.Admin;
using AspNetBase.Core.Contracts.Services.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetBase.Presentation.App.Pages.ManageUsers
{
    public class CreateModel : PageModel
  {
    private readonly IAdminUserService adminUserService;

    public CreateModel(IAdminUserService adminUserService)
    {
      this.adminUserService = adminUserService;
    }

    [BindProperty]
    public RegisterAccountFm Input { get; set; }

    public string ReturnUrl { get; set; }
    public IActionResult OnGet(string returnUrl = null)
    {
      ReturnUrl = returnUrl;
      return Page();
    }
    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
      if (ModelState.IsValid)
      {
        var(accountCreated, _, errorMessages) =
          await adminUserService.CreateAccount(
            Input.Email,
            Input.Password,
            (code, userId) => Url.Page(
              "/Account/ConfirmEmail",
              pageHandler: null,
              values: new { userId, code },
              protocol: "https"));

        if (accountCreated)
          return returnUrl != null
            ? (IActionResult) LocalRedirect(returnUrl)
            : RedirectToPage("./Index");

        errorMessages.ForEach(msg => ModelState.AddModelError(string.Empty, msg));
      }

      return Page();
    }
  }
}
