using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetBase.Core.Contracts.Services.Identity.AccountManagement
{
  public interface IManageAuthenticationService : IIdentityBaseService
  {
    // Disable 2fa:
    Task<bool> CheckUserHas2faEnabled(ClaimsPrincipal loggedInUser);
    Task<bool> Disable2fa(ClaimsPrincipal loggedInUser);

    // Enable 2fa:
    Task < (string sharedKey, string qrCodeUri) > Get2faSharedKeyAndQrCodeUri(ClaimsPrincipal loggedInUser, AppUser user = null, string authUriFormat = null, string qrCodeIssuer = null);
    Task < (bool isTokenValid, IEnumerable<string> recoveryCodes) > Enable2fa(ClaimsPrincipal loggedInUser, string verificationCode, AppUser user = null);
    Task<IEnumerable<string>> GenerateNew2faRecoveryCodes(ClaimsPrincipal loggedInUser);
    Task<bool> Reset2fa(ClaimsPrincipal loggedInUser);

    // External logins:
    Task < (IList<UserLoginInfo> currentLogins, IList<UserLoginInfo> otherLogins, bool enableLoginRemoval) > GetUserLogins(ClaimsPrincipal loggedInUser);
    Task<bool> RemoveExternalLogin(ClaimsPrincipal loggedInUser);
    Task<ChallengeResult> ChallengeExternalLogin(ClaimsPrincipal loggedInUser, string externalLoginProvider, string redirectUrl);
    Task<bool> LinkExternalLogin(ClaimsPrincipal loggedInUser);

    Task < (bool hasAuthenticator, bool is2FaEnabled, bool isMachineRemembered, int recoveryCodesLeft) > Get2faInfo(ClaimsPrincipal loggedInUser);
    Task<bool> Forget2fa(ClaimsPrincipal loggedInUser);
  }
}
