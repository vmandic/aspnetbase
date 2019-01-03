using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetBase.Common.Utils.Attributes;
using AspNetBase.Core.Contracts.Services.Identity;
using AspNetBase.Core.Models.Identity;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace AspNetBase.Core.Providers.Services.Identity
{
  [RegisterDependency(ServiceLifetime.Scoped)]
  public class ExternalSignInService : IdentityBaseService<ExternalSignInService>, IExternalSignInService
  {
    public ExternalSignInService(
      ILogger<ExternalSignInService> logger,
      UserManager<AppUser> userManager,
      SignInManager<AppUser> signInManager) : base(logger, userManager, signInManager) { }

    public ChallengeResult ChallengeExternalLoginProvider(string provider, string redirectUrl)
    {
      if (string.IsNullOrWhiteSpace(provider))
        throw new ArgumentException("Invalid argument value provided.", nameof(provider));

      if (string.IsNullOrWhiteSpace(redirectUrl))
        throw new ArgumentException("Invalid argument value provided.", nameof(redirectUrl));

      // Request a redirect to the external login provider.
      var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
      return new ChallengeResult(provider, properties);
    }

    public async Task < (SignInResult, IExternalLoginModel) > SignInWithExternalProvider(string remoteError = null)
    {
      if (remoteError != null)
        return (
          SignInResult.Failed,
          new ExternalLoginDto($"Error from external provider: {remoteError}"));

      var info = await signInManager.GetExternalLoginInfoAsync();
      if (info == null)
        return (
          SignInResult.Failed,
          new ExternalLoginDto("Error loading external login information."));

      // Sign in the user with this external login provider if the user already has a login.
      var result = await signInManager.ExternalLoginSignInAsync(
        info.LoginProvider,
        info.ProviderKey,
        isPersistent : false,
        bypassTwoFactor : true);

      if (result.Succeeded)
      {
        logger.LogInformation(
          "{Name} logged in with {LoginProvider} provider.",
          info.Principal.Identity.Name,
          info.LoginProvider);

        return (result, null);
      }

      if (result.IsLockedOut)
        return (result, null);

      // If the user does not have an account, then ask the user to create an account.
      ExternalLoginInputModel input = null;

      if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
      {
        input = new ExternalLoginInputModel
        {
        Email = info.Principal.FindFirstValue(ClaimTypes.Email)
        };
      }

      return (
        SignInResult.NotAllowed,
        new ExternalLoginDto { LoginProvider = info.LoginProvider, Input = input });
    }

    public async Task < (IdentityResult, IExternalLoginModel, ICollection<string> errorMessages) > ConfirmExternalLogin(string email)
    {
      if (string.IsNullOrWhiteSpace(email))
      {
        throw new ArgumentException("message", nameof(email));
      }

      var errorMessages = new List<string>();

      // Get the information about the user from the external login provider
      var info = await signInManager.GetExternalLoginInfoAsync();
      if (info == null)
      {
        return (
          IdentityResult.Failed(),
          new ExternalLoginDto("Error loading external login information during confirmation."),
          errorMessages);
      }

      var user = new AppUser { UserName = email, Email = email };
      var result = await userManager.CreateAsync(user);

      if (result.Succeeded)
      {
        result = await userManager.AddLoginAsync(user, info);

        if (result.Succeeded)
        {
          await signInManager.SignInAsync(user, isPersistent : false);
          logger.LogInformation(
            "User created an account using {Name} provider.",
            info.LoginProvider);

          return (result, null, errorMessages);
        }

        foreach (var error in result.Errors)
          errorMessages.Add(error.Description);
      }

      return (
        IdentityResult.Failed(),
        new ExternalLoginDto { LoginProvider = info.LoginProvider },
        errorMessages);
    }

    public Task<IEnumerable<AuthenticationScheme>> GetExternalLoginProviders()
    {
      return signInManager.GetExternalAuthenticationSchemesAsync();
    }
  }
}
