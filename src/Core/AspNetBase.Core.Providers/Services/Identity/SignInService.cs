using AspNetBase.Common.Utils.Attributes;
using AspNetBase.Core.Contracts.Services.Identity;
using AspNetBase.Core.Models.Identity;
using AspNetBase.Infrastructure.DataAccess.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
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
      IUrlHelper urlHelper,
      IHttpContextAccessor httpContextAccessor) : base(logger, userManager, signInManager, urlHelper)
    {
      this._httpContextAccessor = httpContextAccessor;
    }


    public async Task<(SignInResult, ICollection<string> errorMessages)> SignInWithPassword(LoginDto loginDto)
    {
      var result = await signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, loginDto.RememberMe, loginDto.LockoutOnFailure);

      if (result == SignInResult.Success) logger.LogInformation("User logged in.");
      else if (result.RequiresTwoFactor) logger.LogInformation("User requires two factor authentication.");
      else if (result.IsLockedOut) logger.LogWarning("User account locked out.");

      if (result == SignInResult.Failed || result == SignInResult.NotAllowed)
      {
        logger.LogWarning("User login failed.");
        return (result, new List<string> { "Invalid login attempt." });
      }
      else
      {
        logger.LogWarning("User login successful.");
        return (result, null);
      }
    }

    public Task SignOut(string authenticationScheme)
    {
      return _httpContextAccessor.HttpContext.SignOutAsync(authenticationScheme);
    }
  }
}
