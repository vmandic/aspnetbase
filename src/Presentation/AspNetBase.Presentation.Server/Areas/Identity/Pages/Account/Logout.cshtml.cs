using AspNetBase.Core.Contracts.Services.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace AspNetBase.Presentation.Server.Areas.Identity.Pages.Account
{
  [AllowAnonymous]
  public class LogoutModel : PageModel
  {
    private readonly ISignInService _signInService;

    public LogoutModel(ISignInService signInService)
    {
      this._signInService = signInService;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost(string returnUrl = null)
    {
      await _signInService.SignOut();

      return (returnUrl != null
        ? LocalRedirect(returnUrl)
        : (IActionResult)Page());
    }
  }
}
