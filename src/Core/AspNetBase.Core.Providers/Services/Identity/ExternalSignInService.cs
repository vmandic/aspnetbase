using AspNetBase.Common.Utils.Attributes;
using AspNetBase.Core.Contracts.Services.Identity;
using AspNetBase.Core.Models.Identity;
using AspNetBase.Infrastructure.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace AspNetBase.Core.Providers.Services.Identity
{
  [RegisterDependency(ServiceLifetime.Scoped)]
  public class ExternalSignInService : IdentityBaseService<ExternalSignInService>, IExternalSignInService
  {
    public ExternalSignInService(
      ILogger<ExternalSignInService> logger,
      UserManager<AppUser> userManager,
      SignInManager<AppUser> signInManager,
      IUrlHelper urlHelper) : base(logger, userManager, signInManager, urlHelper) { }

    public ChallengeResult ChallengeExternalLoginProvider(string provider, string redirectUrl)
    {
      // Request a redirect to the external login provider.
      var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
      return new ChallengeResult(provider, properties);
    }

    public async Task<(SignInResult, IExternalLoginModel)> SignInWithExternalProvider(string rootUrl, string returnUrl = null, string remoteError = null)
    {
      returnUrl = returnUrl ?? rootUrl;

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
        isPersistent: false,
        bypassTwoFactor: true);

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
        new ExternalLoginDto(returnUrl, info.LoginProvider, input));
    }

    public async Task<(IdentityResult, IExternalLoginModel)> ConfirmExternalLogin(ModelStateDictionary modelState, string email, string returnUrl = null)
    {
      returnUrl = returnUrl ?? urlHelper.Content("~/");

      // Get the information about the user from the external login provider
      var info = await signInManager.GetExternalLoginInfoAsync();
      if (info == null)
      {
        return (
          IdentityResult.Failed(),
          new ExternalLoginDto("Error loading external login information during confirmation."));
      }

      if (modelState.IsValid)
      {
        var user = new AppUser { UserName = email, Email = email };
        var result = await userManager.CreateAsync(user);

        if (result.Succeeded)
        {
          result = await userManager.AddLoginAsync(user, info);

          if (result.Succeeded)
          {
            await signInManager.SignInAsync(user, isPersistent: false);
            logger.LogInformation(
              "User created an account using {Name} provider.",
              info.LoginProvider);

            return (result, null);
          }
        }

        foreach (var error in result.Errors)
        {
          modelState.AddModelError(string.Empty, error.Description);
        }
      }

      return (
          IdentityResult.Failed(),
          new ExternalLoginDto(returnUrl, info.LoginProvider));
    }

    public Task<IEnumerable<string>> GetExternalLoginProviders()
    {
      throw new System.NotImplementedException();
    }
  }
}
