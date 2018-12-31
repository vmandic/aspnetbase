using AspNetBase.Infrastructure.DataAccess.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AspNetBase.Core.Providers.Services.Identity
{
  public abstract class IdentityBaseService<TServiceImpl>
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
  }
}