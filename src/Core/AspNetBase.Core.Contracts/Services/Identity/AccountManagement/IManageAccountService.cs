using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AspNetBase.Core.Contracts.Services.Identity.AccountManagement
{
  public interface IManageAccountService
  {
    // Account Password:
    Task<bool> CheckUserHasPassword(ClaimsPrincipal loggedInUser);
    Task<bool> ChangeUserPassword(ClaimsPrincipal loggedInUser, string oldPassword, string newPassword);
    Task<bool> SetUserNewPassword(ClaimsPrincipal loggedInUser, string newPassword);

    // Account Data:
    Task<bool> CheckUserPassword(ClaimsPrincipal loggedInUser);
    Task<bool> DeleteAccount(ClaimsPrincipal loggedInUser);
    Task<IDictionary<string, string>> GetPersonalData(ClaimsPrincipal loggedInUser);

    // Recovery Codes:

  }
}
