using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;

namespace AspNetBase.Core.Contracts.Services.Identity.AccountManagement
{
  public interface IManageAccountService : IIdentityBaseService
  {
    // Account Password:
    Task<bool> CheckUserHasPassword(ClaimsPrincipal loggedInUser, AppUser user = null);
    Task<bool> ChangeUserPassword(ClaimsPrincipal loggedInUser, string oldPassword, string newPassword);
    Task<(bool passwordSet, IEnumerable<string> errors)> SetUserNewPassword(ClaimsPrincipal loggedInUser, string newPassword);

    // Account Data:
    Task<bool> CheckUserPassword(ClaimsPrincipal loggedInUser, string password, AppUser user = null);
    Task<bool> DeleteAccount(ClaimsPrincipal loggedInUser, AppUser user = null);
    Task<IDictionary<string, string>> GetPersonalData(ClaimsPrincipal loggedInUser);
  }
}
