using System;
using System.Linq;
using System.Threading.Tasks;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetBase.Presentation.App.Pages.ManageUsers
{
  public class EditModel : PageModel
  {
    private readonly UserManager<AppUser> userManager;

    public EditModel(UserManager<AppUser> userManager)
    {
      this.userManager = userManager;
    }

    [BindProperty]
    public AppUser AppUser { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
      if (id == null)
        return NotFound();

      AppUser = await userManager.FindByIdAsync(id.ToString());

      if (AppUser == null)
        return NotFound();

      return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
      if (!ModelState.IsValid)
        return Page();

      var result = await userManager.UpdateAsync(AppUser);

      if (!result.Succeeded)
      {
        result.Errors.ForEach(e => ModelState.AddModelError("All", e.Description));
        return Page();
      }

      return RedirectToPage("./Index");
    }
  }
}
