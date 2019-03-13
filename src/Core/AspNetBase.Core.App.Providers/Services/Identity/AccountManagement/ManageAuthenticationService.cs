using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AspNetBase.Common.Utils.Attributes;
using AspNetBase.Common.Utils.Extensions;
using AspNetBase.Core.App.Contracts.Services.Identity;
using AspNetBase.Core.App.Contracts.Services.Identity.AccountManagement;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNetBase.Core.App.Providers.Services.Identity.AccountManagement
{
  [RegisterDependency(ServiceLifetime.Scoped)]
  public class ManageAuthenticationService : IdentityBaseService<ManageAuthenticationService>, IManageAuthenticationService
  {
    const string AUTH_URI_FORMAT = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
    const string QR_CODE_ISSUER = "AspNetBase.Presentation.App";
    const int NO_RECOVERY_CODES_TO_GEN = 10;

    private readonly ISignInService _signInService;
    private readonly UrlEncoder _urlEncoder;

    public ManageAuthenticationService(
      ILogger<ManageAuthenticationService> logger,
      UserManager<AppUser> userManager,
      SignInManager<AppUser> signInManager,
      ISignInService signInService,
      UrlEncoder urlEncoder) : base(logger, userManager, signInManager)
    {
      _signInService = signInService;
      _urlEncoder = urlEncoder;
    }

    public async Task<ChallengeResult> ChallengeExternalLogin(ClaimsPrincipal loggedInUser, string externalLoginProvider, string redirectUrl)
    {
      if (loggedInUser == null)
        throw new ArgumentNullException(nameof(loggedInUser));

      if (string.IsNullOrWhiteSpace(externalLoginProvider))
        throw new ArgumentException("Invalid argument value provided.", nameof(externalLoginProvider));

      if (string.IsNullOrWhiteSpace(redirectUrl))
        throw new ArgumentException("Invalid argument value provided.", nameof(redirectUrl));

      // Clear the existing external cookie to ensure a clean login process
      await _signInService.SignOut(IdentityConstants.ExternalScheme);

      // Request a redirect to the external login provider to link a login for the current user
      var authProps = signInManager.ConfigureExternalAuthenticationProperties(
        externalLoginProvider,
        redirectUrl,
        loggedInUser.GetUserId().ToString());

      return new ChallengeResult(externalLoginProvider, authProps);
    }

    public async Task<bool> CheckUserHas2faEnabled(ClaimsPrincipal loggedInUser, AppUser user = null)
    {
      user = user ?? await GetUserOrThrow(loggedInUser);
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

    public async Task < (bool isTokenValid, IEnumerable<string> recoveryCodes) > Enable2fa(
      ClaimsPrincipal loggedInUser,
      string tokenVerificationCode,
      AppUser user = null)
    {
      if (string.IsNullOrWhiteSpace(tokenVerificationCode))
        throw new ArgumentException("Invalid arugment value provided.", nameof(tokenVerificationCode));

      var badResult = (false, Enumerable.Empty<string>());

      // Strip spaces and hypens
      tokenVerificationCode = tokenVerificationCode
        .Replace(" ", string.Empty)
        .Replace("-", string.Empty);

      if (!await CheckIs2faTokenValid(tokenVerificationCode, user))
        return badResult;

      var result = await userManager.SetTwoFactorEnabledAsync(user, true);

      if (result.Succeeded)
      {
        logger.LogInformation("User with ID '{UserId}' has enabled 2FA with an authenticator app.", user.Id);
        return (true, await TryGenerateRecoveryCodes(user));
      }

      return badResult;
    }

    private async Task<bool> CheckIs2faTokenValid(string verificationCode, AppUser user) =>
      await userManager.VerifyTwoFactorTokenAsync(
        user,
        userManager.Options.Tokens.AuthenticatorTokenProvider,
        verificationCode);

    private async Task<IEnumerable<string>> TryGenerateRecoveryCodes(AppUser user) =>
      await userManager.CountRecoveryCodesAsync(user) == 0 ?
      await userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, NO_RECOVERY_CODES_TO_GEN) :
      Enumerable.Empty<string>();

    public async Task<bool> Forget2fa(ClaimsPrincipal loggedInUser)
    {
      await GetUserOrThrow(loggedInUser);
      await signInManager.ForgetTwoFactorClientAsync();
      return true;
    }

    public async Task<IEnumerable<string>> GenerateNew2faRecoveryCodes(
      ClaimsPrincipal loggedInUser,
      int? numberOfCodesToGenerate = null,
      AppUser user = null)
    {
      user = user ?? await GetUserOrThrow(loggedInUser);

      if (numberOfCodesToGenerate < 1)
        throw new ArgumentOutOfRangeException(nameof(numberOfCodesToGenerate));

      var codes = await userManager.GenerateNewTwoFactorRecoveryCodesAsync(
        user,
        numberOfCodesToGenerate ?? NO_RECOVERY_CODES_TO_GEN);

      logger.LogInformation(
        "User with ID '{UserId}' has generated new 2FA recovery codes.",
        user.Id);

      return codes;
    }

    public async Task < (bool hasAuthenticator, bool is2FaEnabled, bool isMachineRemembered, int recoveryCodesLeft) > Get2faInfo(ClaimsPrincipal loggedInUser)
    {
      var user = await GetUserOrThrow(loggedInUser);

      return (
        await userManager.GetAuthenticatorKeyAsync(user) != null,
        await userManager.GetTwoFactorEnabledAsync(user),
        await signInManager.IsTwoFactorClientRememberedAsync(user),
        await userManager.CountRecoveryCodesAsync(user)
      );
    }

    public async Task < (IList<UserLoginInfo> currentLogins, IList<AuthenticationScheme> otherLogins, bool enableLoginRemoval) > GetUserLogins(ClaimsPrincipal loggedInUser)
    {
      var user = await GetUserOrThrow(loggedInUser);
      var currentLogins = await userManager.GetLoginsAsync(user);

      return (
        currentLogins,
        otherLogins: (await signInManager
          .GetExternalAuthenticationSchemesAsync())
        .Where(auth => currentLogins.All(ul => auth.Name != ul.LoginProvider))
        .ToList(),
        enableLoginRemoval : user.PasswordHash != null || currentLogins.Count > 1
      );
    }

    public async Task<bool> LinkExternalLogin(ClaimsPrincipal loggedInUser)
    {
      var user = await GetUserOrThrow(loggedInUser);
      var info = await signInManager.GetExternalLoginInfoAsync(await userManager.GetUserIdAsync(user));

      if (info == null)
        return false;

      var result = await userManager.AddLoginAsync(user, info);

      if (!result.Succeeded)
        return false;

      // Clear the existing external cookie to ensure a clean login process
      await _signInService.SignOut(IdentityConstants.ExternalScheme);

      return true;
    }

    public async Task<bool> RemoveExternalLogin(ClaimsPrincipal loggedInUser, string loginProvider, string providerKey)
    {
      var user = await GetUserOrThrow(loggedInUser);

      if (string.IsNullOrWhiteSpace(loginProvider))
        throw new ArgumentException("Invalid argument value provided.", nameof(loginProvider));

      if (string.IsNullOrWhiteSpace(providerKey))
        throw new ArgumentException("Invalid argument value provided.", nameof(providerKey));

      var result = await userManager.RemoveLoginAsync(user, loginProvider, providerKey);

      if (!result.Succeeded)
        return false;

      await signInManager.RefreshSignInAsync(user);
      return true;
    }

    public async Task<bool> Reset2fa(ClaimsPrincipal loggedInUser)
    {
      var user = await GetUserOrThrow(loggedInUser);

      await userManager.SetTwoFactorEnabledAsync(user, false);
      await userManager.ResetAuthenticatorKeyAsync(user);

      logger.LogInformation("User with ID '{UserId}' has reset their authentication app key.", user.Id);

      await signInManager.RefreshSignInAsync(user);
      return true;
    }

    public async Task < (string sharedKey, string qrCodeUri) > Get2faSharedKeyAndQrCodeUri(
      ClaimsPrincipal loggedInUser,
      AppUser user = null,
      string authUriFormat = null,
      string qrCodeIssuer = null)
    {
      user = user ?? await GetUserOrThrow(loggedInUser);

      authUriFormat = authUriFormat ?? AUTH_URI_FORMAT;
      qrCodeIssuer = qrCodeIssuer ?? QR_CODE_ISSUER;

      // Load the authenticator key & QR code URI to display on the form
      var unformattedKey = await userManager.GetAuthenticatorKeyAsync(user);

      if (string.IsNullOrWhiteSpace(unformattedKey))
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
      if (unformattedKey == null)
        throw new ArgumentNullException(nameof(unformattedKey));

      var result = new StringBuilder();
      int currentPosition = 0;

      while (currentPosition + 4 < unformattedKey.Length)
      {
        result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
        currentPosition += 4;
      }

      if (currentPosition < unformattedKey.Length)
        result.Append(unformattedKey.Substring(currentPosition));

      return result.ToString().ToLowerInvariant();
    }

    private string GenerateQrCodeUri(
      string email,
      string unformattedKey,
      string authenticatorUriFormat,
      string qrCodeIssuer)
    {
      if (string.IsNullOrWhiteSpace(email))
      {
        throw new ArgumentException("Invalid argument value provided.", nameof(email));
      }

      if (string.IsNullOrWhiteSpace(unformattedKey))
      {
        throw new ArgumentException("Invalid argument value provided.", nameof(unformattedKey));
      }

      if (string.IsNullOrWhiteSpace(authenticatorUriFormat))
      {
        throw new ArgumentException("Invalid argument value provided.", nameof(authenticatorUriFormat));
      }

      if (string.IsNullOrWhiteSpace(qrCodeIssuer))
      {
        throw new ArgumentException("Invalid argument value provided.", nameof(qrCodeIssuer));
      }

      return string.Format(
        authenticatorUriFormat,
        _urlEncoder.Encode(qrCodeIssuer),
        _urlEncoder.Encode(email),
        unformattedKey);
    }
  }
}
