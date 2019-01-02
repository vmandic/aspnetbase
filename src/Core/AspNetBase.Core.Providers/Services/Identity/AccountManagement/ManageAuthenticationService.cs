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

    public async Task<bool> CheckUserHas2faEnabled(ClaimsPrincipal loggedInUser)
    {
      var user = await GetUserOrThrow(loggedInUser);
      return await userManager.GetTwoFactorEnabledAsync(user);
    }

    public async Task<bool> Disable2fa(ClaimsPrincipal loggedInUser)
    {
      var user = await GetUserOrThrow(loggedInUser);
      var result = await userManager.SetTwoFactorEnabledAsync(user, false);

      if (!result.Succeeded)
      {
        return false;
      }

      logger.LogInformation("User with ID '{UserId}' has disabled 2fa.", user.Id);
      return true;
    }

    public Task < (IEnumerable<string> recoveryCodes, IEnumerable<string> errors) > Enable2fa(ClaimsPrincipal loggedInUser)
    {
      throw new System.NotImplementedException();
    }

    public Task<bool> Forget2fa(ClaimsPrincipal loggedInUser)
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

    public Task<bool> LinkExternalLogin(ClaimsPrincipal loggedInUser)
    {
      throw new System.NotImplementedException();
    }

    public Task<bool> RemoveExternalLogin(ClaimsPrincipal loggedInUser)
    {
      throw new System.NotImplementedException();
    }

    public Task<bool> Reset2fa(ClaimsPrincipal loggedInUser)
    {
      throw new System.NotImplementedException();
    }
  }
}
