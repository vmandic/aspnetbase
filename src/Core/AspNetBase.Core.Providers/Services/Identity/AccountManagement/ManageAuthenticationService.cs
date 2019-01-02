using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
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
    const string AUTH_URI_FORMAT = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
    const string QR_CODE_ISSUER = "AspNetBase.Presentation.Server";
    private readonly UrlEncoder _urlEncoder;

    public ManageAuthenticationService(
      ILogger<ManageAuthenticationService> logger,
      UserManager<AppUser> userManager,
      SignInManager<AppUser> signInManager,
      UrlEncoder urlEncoder) : base(logger, userManager, signInManager)
    {
      _urlEncoder = urlEncoder;
    }

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

    public async Task < (bool isTokenValid, IEnumerable<string> recoveryCodes) > Enable2fa(ClaimsPrincipal loggedInUser, string verificationCode, AppUser user = null)
    {
      var badResult = (false, Enumerable.Empty<string>());
      // Strip spaces and hypens
      verificationCode = verificationCode
        .Replace(" ", string.Empty)
        .Replace("-", string.Empty);

      if (!await CheckIs2faTokenValid(verificationCode, user))
        return badResult;

      var result = await userManager.SetTwoFactorEnabledAsync(user, true);

      if (result.Succeeded)
      {
        logger.LogInformation("User with ID '{UserId}' has enabled 2FA with an authenticator app.", user.Id);

        return (true, await TryGenerateRecoveryCodes(user));
      }

      return badResult;
    }

    private async Task<bool> CheckIs2faTokenValid(string verificationCode, AppUser user)
    {
      return await userManager.VerifyTwoFactorTokenAsync(
        user,
        userManager.Options.Tokens.AuthenticatorTokenProvider,
        verificationCode);
    }

    private async Task<IEnumerable<string>> TryGenerateRecoveryCodes(AppUser user) =>
      await userManager.CountRecoveryCodesAsync(user) == 0 ?
      await userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10) :
      Enumerable.Empty<string>();

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

    public async Task < (string sharedKey, string qrCodeUri) > Get2faSharedKeyAndQrCodeUri(ClaimsPrincipal loggedInUser, AppUser user = null, string authUriFormat = null, string qrCodeIssuer = null)
    {
      user = user ?? await GetUserOrThrow(loggedInUser);

      authUriFormat = authUriFormat ?? AUTH_URI_FORMAT;
      qrCodeIssuer = qrCodeIssuer ?? QR_CODE_ISSUER;

      // Load the authenticator key & QR code URI to display on the form
      var unformattedKey = await userManager.GetAuthenticatorKeyAsync(user);

      if (string.IsNullOrEmpty(unformattedKey))
      {
        await userManager.ResetAuthenticatorKeyAsync(user);
        unformattedKey = await userManager.GetAuthenticatorKeyAsync(user);
      }

      return (
        sharedKey: FormatKey(unformattedKey),
        qrCodeUri: GenerateQrCodeUri(
          await userManager.GetEmailAsync(user),
          unformattedKey,
          authUriFormat,
          qrCodeIssuer)
      );
    }

    private string FormatKey(string unformattedKey)
    {
      var result = new StringBuilder();
      int currentPosition = 0;

      while (currentPosition + 4 < unformattedKey.Length)
      {
        result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
        currentPosition += 4;
      }

      if (currentPosition < unformattedKey.Length)
      {
        result.Append(unformattedKey.Substring(currentPosition));
      }

      return result.ToString().ToLowerInvariant();
    }

    private string GenerateQrCodeUri(
      string email,
      string unformattedKey,
      string authenticatorUriFormat,
      string qrCodeIssuer)
    {
      return string.Format(
        authenticatorUriFormat,
        _urlEncoder.Encode(qrCodeIssuer),
        _urlEncoder.Encode(email),
        unformattedKey);
    }
  }
}
