using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AspNetBase.Common.Utils.Attributes;
using AspNetBase.Core.Contracts.Services.Identity;
using AspNetBase.Infrastructure.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNetBase.Core.Providers.Services.Identity
{
  [RegisterDependency(ServiceLifetime.Scoped)]
  public class TwoFactorAuthService : IdentityBaseService<TwoFactorAuthService>, ITwoFactorAuthService
  {
    public TwoFactorAuthService(
      ILogger<TwoFactorAuthService> logger,
      UserManager<AppUser> userManager,
      SignInManager<AppUser> signInManager)
      : base(logger, userManager, signInManager) { }

    public Task EnsureUserForTwoFactorAuthentication()
    {
      return GetTwoFactorAuthenticationUser();
    }

    public async Task<AppUser> GetTwoFactorAuthenticationUser()
    {
      var user = await signInManager.GetTwoFactorAuthenticationUserAsync();

      if (user == null)
        throw new InvalidOperationException($"Unable to load two-factor authentication user.");

      return user;
    }

    public async Task<SignInResult> SignIn(string authCode, bool rememberMe, bool rememberMachine)
    {
      var result = await signInManager.TwoFactorAuthenticatorSignInAsync(
        authCode,
        rememberMe,
        rememberMachine);

      if (result.Succeeded)
      {
        logger.LogInformation("User logged in with 2fa.");
        return result;
      }
      else if (result.IsLockedOut)
      {
        logger.LogWarning("User with 2fa sign in attempt is locked out.");
        return result;
      }

      logger.LogWarning("Invalid authenticator code entered on user 2fa sign in.");
      return SignInResult.Failed;
    }
  }
}
