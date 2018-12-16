using AspNetBase.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AspNetBase.BusinessLogicProviders.Services.Identity
{
  public abstract class IdentityBaseService<TServiceImpl>
  {
    protected readonly ILogger<TServiceImpl> logger;
    protected readonly UserManager<AppUser> userManager;
    protected readonly SignInManager<AppUser> signInManager;
    protected readonly IUrlHelper urlHelper;

    protected IdentityBaseService(
      ILogger<TServiceImpl> logger,
      UserManager<AppUser> userManager,
      SignInManager<AppUser> signInManager,
      IUrlHelper urlHelper)
    {
      this.logger = logger;
      this.userManager = userManager;
      this.signInManager = signInManager;
      this.urlHelper = urlHelper;
    }
  }
}
