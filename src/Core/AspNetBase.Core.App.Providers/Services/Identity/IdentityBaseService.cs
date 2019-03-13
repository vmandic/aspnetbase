using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetBase.Core.App.Contracts.Services.Identity;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AspNetBase.Core.App.Providers.Services.Identity
{
  public abstract class IdentityBaseService<TServiceImpl> : IIdentityBaseService
  {
    protected readonly ILogger<TServiceImpl> logger;
    protected readonly UserManager<AppUser> userManager;
    protected readonly SignInManager<AppUser> signInManager;

    protected IdentityBaseService(
      ILogger<TServiceImpl> logger,
      UserManager<AppUser> userManager,
      SignInManager<AppUser> signInManager)
    {
      this.logger = logger;
      this.userManager = userManager;
      this.signInManager = signInManager;
    }

    public async Task<AppUser> GetUserOrThrow(ClaimsPrincipal loggedInUser)
    {
      if (loggedInUser == null)
      {
        throw new ArgumentNullException(nameof(loggedInUser));
      }

      var user = await userManager.GetUserAsync(loggedInUser);
      if (user == null)
      {
        throw new InvalidOperationException("Unable to load user for the given claims principal.");
      }

      return user;
    }
  }
}
