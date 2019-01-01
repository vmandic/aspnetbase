using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetBase.Core.Contracts.Services.Identity.AccountManagement
{
  public interface IManageAuthenticationService
  {
    // Disable 2fa:
    Task<bool> CheckUserHas2faEnabled(ClaimsPrincipal loggedInUser);
    Task Disable2fa(ClaimsPrincipal loggedInUser);

    // Enable 2fa:
    Task < (string sharedKey, string qrCodeUri) > Get2faSharedKeyAndQrCodeUri();
    Task < (IEnumerable<string> recoveryCodes, IEnumerable<string> errors) > Enable2fa(ClaimsPrincipal loggedInUser);
    Task<IEnumerable<string>> GenerateNew2faRecoveryCodes(ClaimsPrincipal loggedInUser);
    Task Reset2fa(ClaimsPrincipal loggedInUser);

    // External logins:
    Task < (IList<UserLoginInfo> currentLogins, IList<UserLoginInfo> otherLogins, bool enableLoginRemoval) > GetUserLogins(ClaimsPrincipal loggedInUser);
    Task RemoveExternalLogin(ClaimsPrincipal loggedInUser);
    Task<ChallengeResult> ChallengeExternalLogin(ClaimsPrincipal loggedInUser, string externalLoginProvider, string redirectUrl);
    Task LinkExternalLogin(ClaimsPrincipal loggedInUser);

    Task < (bool hasAuthenticator, bool is2FaEnabled, bool isMachineRemembered, int recoveryCodesLeft) > Get2faInfo(ClaimsPrincipal loggedInUser);
    Task Forget2fa(ClaimsPrincipal loggedInUser);
  }
}
