using System.Linq;
using AspNetBase.Core.App.Models.Admin.Roles;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetBase.Presentation.App.Pages.ManageRoles
{
    public class IndexModel : PageModel
  {
    private readonly RoleManager<AppRole> roleManager;

    public IndexModel(RoleManager<AppRole> roleManager)
    {

      this.roleManager = roleManager;
    }

    public IQueryable<RoleListVm> Roles { get; set; }

    public void OnGet()
    {
      Roles = roleManager.Roles.Select(x => new RoleListVm
      {
        Id = x.Id,
        Name = x.Name,
        Users = x.UserRoles.Select(ur => ur.User.Email),
        UserCount = x.UserRoles.Count
      });
    }
  }
}
