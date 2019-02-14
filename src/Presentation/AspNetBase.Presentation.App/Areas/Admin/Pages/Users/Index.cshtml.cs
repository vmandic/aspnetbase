using System.Linq;
using AspNetBase.Core.App.Models.Admin.Users;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AspNetBase.Presentation.App.Pages.ManageUsers
{
  public class IndexModel : PageModel
  {
    private readonly UserManager<AppUser> userManger;

    public IndexModel(UserManager<AppUser> userManger)
    {
      this.userManger = userManger;
    }

    public IQueryable<UserListVm> Users { get; set; }

    public void OnGet()
    {
      Users = userManger.Users.Select(x => new UserListVm
      {
        Id = x.Id,
        Email =x.Email,
        EmailConfirmed = x.EmailConfirmed,
        Roles = x.UserRoles.Select(r => r.Role.Name),
        TwoFaEnabled = x.TwoFactorEnabled
      });
    }
  }
}
