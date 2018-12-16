using System.Security.Claims;
using System.Threading.Tasks;
using AspNetBase.Core.Contracts.Services.Identity;
using AspNetBase.Core.Models.Identity;
using AspNetBase.Infrastructure.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace AspNetBase.Core.Providers.Services.Identity
{
  public class SignInService : IdentityBaseService<SignInService>, ISignInService
  {
    public SignInService(
      ILogger<SignInService> logger,
      UserManager<AppUser> userManager,
      SignInManager<AppUser> signInManager,
      IUrlHelper urlHelper) : base(logger, userManager, signInManager, urlHelper)
    { }

    public IActionResult ChallengeExternalLoginProvider(string provider, string returnUrl = null)
    {
      // Request a redirect to the external login provider.
      var redirectUrl = urlHelper.Page("./ExternalLogin", pageHandler: "Callback", values : new { returnUrl });
      var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

      return new ChallengeResult(provider, properties);
    }

    public async Task < (IActionResult, IExternalLoginModel) > LoginWithExternalProvider(string returnUrl = null, string remoteError = null)
    {
      returnUrl = returnUrl ?? urlHelper.Content("~/");

      if (remoteError != null)
      {
        return (
          new RedirectToPageResult("./Login", new { ReturnUrl = returnUrl }),
          new ExternalLoginDto($"Error from external provider: {remoteError}"));
      }

      var info = await signInManager.GetExternalLoginInfoAsync();
      if (info == null)
      {
        return (
          new RedirectToPageResult("./Login", new { ReturnUrl = returnUrl }),
          new ExternalLoginDto("Error loading external login information."));
      }

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

        return (new LocalRedirectResult(returnUrl), null);
      }

      if (result.IsLockedOut)
      {
        return (new RedirectToPageResult("./Lockout"), null);
      }
      else
      {
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
          new PageResult(),
          new ExternalLoginDto(returnUrl, info.LoginProvider, input));
      }
    }
  }
}
