using System.Security.Claims;
using System.Threading.Tasks;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;

namespace AspNetBase.Core.Contracts.Services.Identity
{
  public interface IIdentityBaseService
  {
    Task<AppUser> GetUserOrThrow(ClaimsPrincipal loggedInUser);
  }
}
