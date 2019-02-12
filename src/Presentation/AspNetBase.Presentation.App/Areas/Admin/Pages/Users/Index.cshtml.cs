using System.Linq;
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

    public IQueryable<AppUser> Users { get; set; }

    public void OnGet()
    {
      Users = userManger.Users.Include(x => x.UserRoles).ThenInclude(x => x.Role);
    }
  }
}
