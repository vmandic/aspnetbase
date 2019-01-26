using System.Threading.Tasks;
using AspNetBase.Common.Utils.Extensions;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetBase.Presentation.App.Areas.Identity.Pages.Account.Manage
{
    public class PersonalDataModel : PageModel
  {
    private readonly UserManager<AppUser> _userManager;

    public PersonalDataModel(
      UserManager<AppUser> userManager)
    {
      _userManager = userManager;
    }

    public async Task<IActionResult> OnGet()
    {
      var user = await _userManager.GetUserAsync(User);
      if (user == null)
      {
        return NotFound($"Unable to load user with ID '{User.GetUserId()}'.");
      }

      return Page();
    }
  }
}
