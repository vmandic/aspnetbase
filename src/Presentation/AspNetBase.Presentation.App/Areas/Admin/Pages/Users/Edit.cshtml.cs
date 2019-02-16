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
    private readonly AppDbContext db;

    public EditModel(UserManager<AppUser> userManager, AppDbContext db)
    {
      this.userManager = userManager;
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
      LoadRolesDropDown();

      return Page();
    }

    private IEnumerable<SelectListItem> LoadRolesDropDown()
    {
      return (Roles = db.Roles.Select(x => new SelectListItem
      {
        Value = x.Id.ToString(),
          Text = x.Name
      }));
    }

    public async Task<IActionResult> OnPostAsync()
    {
      if (!ModelState.IsValid)
        return Page();

      var user = await userManager.FindByIdAsync(UserEditModel.Id.ToString());

      if (user == null)
        return NotFound();

      var updateUserResult = await UpdateUserDetails(user);
      updateUserResult.Errors.ForEach(e => ModelState.AddModelError(string.Empty, e.Description));

      var roles = LoadRolesDropDown();
      await UpdateRoles(user, updateUserResult, roles.Select(x => (int.Parse(x.Value), x.Text)));

      if (!ModelState.IsValid)
      {
        UserEditModel = new UserEditModel(user);
        return Page();
      }

      return RedirectToPage("./Index");
    }

    private Task<IdentityResult> UpdateUserDetails(AppUser user)
    {
      user.PhoneNumber = UserEditModel.PhoneNumber;
      user.LockoutEnd = UserEditModel.LockoutEnd;
      user.LockoutEnabled = UserEditModel.LockoutEnabled;
      user.AccessFailedCount = UserEditModel.AccessFailedCount;

      return userManager.UpdateAsync(user);
    }

    private async Task UpdateRoles(AppUser user, IdentityResult updateUserResult, IEnumerable < (int id, string name) > allRoles)
    {
      if (updateUserResult.Succeeded)
      {
        var addRolesResult = await AddRoles(user, allRoles);
        addRolesResult.Errors.ForEach(e => ModelState.AddModelError(string.Empty, e.Description));

        if (addRolesResult.Succeeded)
        {
          var removeRolesResult = await RemoveRoles(user, allRoles);
          removeRolesResult.Errors.ForEach(e => ModelState.AddModelError(string.Empty, e.Description));
        }
      }
    }

    private Task<IdentityResult> RemoveRoles(AppUser user, IEnumerable < (int id, string name) > allRoles)
    {
      var rolesToRemove = allRoles
        .Where(role =>
          !UserEditModel.RoleIds.Contains(role.id))
        .Select(x => x.name)
        .ToList();

      return rolesToRemove.Count > 0 ?
        userManager.RemoveFromRolesAsync(user, rolesToRemove) :
        Task.FromResult(IdentityResult.Success);
    }

    private Task<IdentityResult> AddRoles(AppUser user, IEnumerable < (int id, string name) > allRoles)
    {
      var rolesToAdd = allRoles
        .Where(role =>
          UserEditModel.RoleIds.Contains(role.id) &&
          !userManager.IsInRoleAsync(user, role.name).GetAwaiter().GetResult())
        .Select(x => x.name)
        .ToList();

      return rolesToAdd.Count > 0 ?
        userManager.AddToRolesAsync(user, rolesToAdd) :
        Task.FromResult(IdentityResult.Success);
    }
  }
}
