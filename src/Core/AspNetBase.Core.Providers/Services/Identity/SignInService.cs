using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetBase.Common.Utils.Attributes;
using AspNetBase.Core.Contracts.Services.Identity;
using AspNetBase.Core.Models.Identity;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace AspNetBase.Core.Providers.Services.Identity
{
  [RegisterDependency(ServiceLifetime.Scoped)]
  public class SignInService : IdentityBaseService<SignInService>, ISignInService
  {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SignInService(
      ILogger<SignInService> logger,
      UserManager<AppUser> userManager,
      SignInManager<AppUser> signInManager,
      IHttpContextAccessor httpContextAccessor) : base(logger, userManager, signInManager)
    {
      this._httpContextAccessor = httpContextAccessor;
    }

    public async Task < (SignInResult, IEnumerable<string> errorMessages) > SignInWithPassword(LoginDto loginDto)
    {
      // NOTE: uses the default UserClaimsFactory and assigns the default
      // Claims with the default Identity.Application Auth Scheme
      var result = await signInManager.PasswordSignInAsync(
        loginDto.Email,
        loginDto.Password,
        loginDto.RememberMe,
        loginDto.LockoutOnFailure);

      if (result == SignInResult.Success) logger.LogInformation("User logged in.");
      else if (result.RequiresTwoFactor) logger.LogWarning("User requires two factor authentication.");
      else if (result.IsLockedOut) logger.LogWarning("User account locked out.");

      if (result == SignInResult.Failed || result == SignInResult.NotAllowed)
      {
        logger.LogWarning("User sign in with password failed.");
        return (result, new List<string> { "Invalid login attempt." });
      }
      else
      {
        logger.LogInformation("User signed in with password successfully.");
        return (result, Enumerable.Empty<string>());
      }
    }

    public Task SignOut()
    {
      // NOTE: signs out all three auth schemes added by AddIdentity
      return signInManager.SignOutAsync().ContinueWith(t =>
      {
        if (t.Status == TaskStatus.RanToCompletion)
        {
          logger.LogInformation("User signed out successfully.");
        }
      });
    }

    public Task SignOut(string authenticationScheme)
    {
      return _httpContextAccessor.HttpContext.SignOutAsync(authenticationScheme).ContinueWith(t =>
      {
        if (t.Status == TaskStatus.RanToCompletion)
        {
          logger.LogInformation(
            "User signed out successfully for auth scheme: {scheme}",
            authenticationScheme);
        }
      });
    }
  }
}