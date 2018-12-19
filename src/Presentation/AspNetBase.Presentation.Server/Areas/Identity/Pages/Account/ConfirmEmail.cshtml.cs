using AspNetBase.Core.Contracts.Services.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetBase.Presentation.Server.Areas.Identity.Pages.Account
{
  [AllowAnonymous]
  public class ConfirmEmailModel : PageModel
  {
    private readonly ISignInService _signInService;
    private readonly IRegisterAccountService _registerAccountService;

    public ConfirmEmailModel(IRegisterAccountService registerAccountService)
    {
      this._registerAccountService = registerAccountService;
    }

    public async Task<IActionResult> OnGetAsync(int userId, string code)
    {
      if (userId < 1 || code == null)
        return RedirectToPage("/Index");

      var result = await _registerAccountService.ConfirmEmailAddress(userId, code);

      if (!result.Succeeded)
        result.Errors.ForEach(msg => ModelState.AddModelError(string.Empty, msg.Description));

      return Page();
    }
  }
}
