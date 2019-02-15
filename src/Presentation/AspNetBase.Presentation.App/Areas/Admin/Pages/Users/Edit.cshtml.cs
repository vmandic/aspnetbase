using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetBase.Core.App.Models.Admin.Users;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;
using AspNetBase.Infrastructure.DataAccess.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AspNetBase.Presentation.App.Pages.ManageUsers
{
  public class EditModel : PageModel
  {
    private readonly UserManager<AppUser> userManager;
    private readonly RoleManager<AppRole> roleManager;
    private readonly AppDbContext db;

    public EditModel(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, AppDbContext db)
    {
      this.userManager = userManager;
      this.roleManager = roleManager;
      this.db = db;
    }

    [BindProperty]
    public UserEditModel UserEditModel { get; set; }

    public IEnumerable<SelectListItem> Roles { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
      if (id == null)
        return NotFound();

      var user = await db.Users.Include(x => x.UserRoles).FirstOrDefaultAsync(x => x.Id == id);

      if (user == null)
        return NotFound();

      UserEditModel = new UserEditModel(user);
      LoadRoles();

      return Page();
    }

    private void LoadRoles()
    {
      Roles = db.Roles.Select(x => new SelectListItem
      {
        Value = x.Id.ToString(),
          Text = x.Name
      });
    }

    public async Task<IActionResult> OnPostAsync()
    {
      if (!ModelState.IsValid)
        return Page();

      var user = await userManager.FindByIdAsync(UserEditModel.Id.ToString());

      if (user == null)
        return NotFound();

      user.PhoneNumber = UserEditModel.PhoneNumber;
      user.LockoutEnd = UserEditModel.LockoutEnd;
      user.LockoutEnabled = UserEditModel.LockoutEnabled;
      user.AccessFailedCount = UserEditModel.AccessFailedCount;

      var updateUserResult = await userManager.UpdateAsync(user);

      LoadRoles();
      var rolesToAdd = Roles.Where(x => UserEditModel.RoleIds.Contains(int.Parse(x.Value))).Select(x => x.Text);

      // TODO: fix roles management, don't use RoleManager<TRole> maybe?

      foreach (var role in Roles)
      {
        if (UserEditModel.RoleIds.Contains(int.Parse(role.Value)) && !await userManager.IsInRoleAsync(user, role.Text))
        {

        }
      }

      // await userManager.IsInRoleAsync()
      var removeRolesResult = await userManager.RemoveFromRolesAsync(user, Roles.Select(x => x.Text));
      var addRolesResult = await userManager.AddToRolesAsync(user, rolesToAdd);

      if (!updateUserResult.Succeeded)
        updateUserResult.Errors.ForEach(e => ModelState.AddModelError(string.Empty, e.Description));

      if (!removeRolesResult.Succeeded)
        removeRolesResult.Errors.ForEach(e => ModelState.AddModelError(string.Empty, e.Description));

      if (!addRolesResult.Succeeded)
        addRolesResult.Errors.ForEach(e => ModelState.AddModelError(string.Empty, e.Description));

      if (!ModelState.IsValid)
      {
        UserEditModel = new UserEditModel(user);
        return Page();
      }

      return RedirectToPage("./Index");
    }

 }
}
