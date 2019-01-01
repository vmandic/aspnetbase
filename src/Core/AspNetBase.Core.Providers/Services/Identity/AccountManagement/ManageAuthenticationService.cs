using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetBase.Core.Contracts.Services.Identity.AccountManagement;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AspNetBase.Core.Providers.Services.Identity.AccountManagement
{
  public class ManageAuthenticationService : IdentityBaseService<ManageAuthenticationService>, IManageAuthenticationService
  {
    public ManageAuthenticationService(
      ILogger<ManageAuthenticationService> logger,
      UserManager<AppUser> userManager,
      SignInManager<AppUser> signInManager) : base(logger, userManager, signInManager) { }

    public Task<ChallengeResult> ChallengeExternalLogin(ClaimsPrincipal loggedInUser, string externalLoginProvider, string redirectUrl)
    {
      throw new System.NotImplementedException();
    }

    public Task<bool> CheckUserHas2faEnabled(ClaimsPrincipal loggedInUser)
    {
      throw new System.NotImplementedException();
    }

    public Task Disable2fa(ClaimsPrincipal loggedInUser)
    {
      throw new System.NotImplementedException();
    }

    public Task < (IEnumerable<string> recoveryCodes, IEnumerable<string> errors) > Enable2fa(ClaimsPrincipal loggedInUser)
    {
      throw new System.NotImplementedException();
    }

    public Task Forget2fa(ClaimsPrincipal loggedInUser)
    {
      throw new System.NotImplementedException();
    }

    public Task<IEnumerable<string>> GenerateNew2faRecoveryCodes(ClaimsPrincipal loggedInUser)
    {
      throw new System.NotImplementedException();
    }

    public Task < (bool hasAuthenticator, bool is2FaEnabled, bool isMachineRemembered, int recoveryCodesLeft) > Get2faInfo(ClaimsPrincipal loggedInUser)
    {
      throw new System.NotImplementedException();
    }

    public Task < (string sharedKey, string qrCodeUri) > Get2faSharedKeyAndQrCodeUri()
    {
      throw new System.NotImplementedException();
    }

    public Task < (IList<UserLoginInfo> currentLogins, IList<UserLoginInfo> otherLogins, bool enableLoginRemoval) > GetUserLogins(ClaimsPrincipal loggedInUser)
    {
      throw new System.NotImplementedException();
    }

    public Task LinkExternalLogin(ClaimsPrincipal loggedInUser)
    {
      throw new System.NotImplementedException();
    }

    public Task RemoveExternalLogin(ClaimsPrincipal loggedInUser)
    {
      throw new System.NotImplementedException();
    }

    public Task Reset2fa(ClaimsPrincipal loggedInUser)
    {
      throw new System.NotImplementedException();
    }
  }
}
