using System.Threading;
using System.Threading.Tasks;
using AspNetBase.Infrastructure.DataAccess.Data;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AspNetBase.Core.Providers.Services.Identity
{
  public class AppUserStore : UserStore<AppUser, AppRole, AppDbContext, int>
  {
    public AppUserStore(AppDbContext context, IdentityErrorDescriber describer = null) : base(context, describer) { }
  }
}
