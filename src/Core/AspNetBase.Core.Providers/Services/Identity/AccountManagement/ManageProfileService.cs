using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetBase.Common.Utils.Attributes;
using AspNetBase.Core.Contracts.Services.Identity.AccountManagement;
using AspNetBase.Core.Models.Identity.AccountManagement;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNetBase.Core.Providers.Services.Identity.AccountManagement
{
  [RegisterDependency(ServiceLifetime.Scoped)]
  public class ManageProfileService : IdentityBaseService<ManageProfileService>, IManageProfileService
  {
    public ManageProfileService(
      ILogger<ManageProfileService> logger,
      UserManager<AppUser> userManager,
      SignInManager<AppUser> signInManager) : base(logger, userManager, signInManager) { }

    public Task<UserProfileDto> GetUserProfile(ClaimsPrincipal loggedInUser)
    {
      throw new NotImplementedException();
    }

    public Task<bool> SaveUserProfile(UserProfileDto userProfileDto)
    {
      throw new NotImplementedException();
    }

    public Task SendVerificationEmail(ClaimsPrincipal loggedInUser, Func<string, int, string> getCallbackUrl)
    {
      throw new NotImplementedException();
    }
  }
}
