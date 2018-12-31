using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetBase.Common.Utils.Attributes;
using AspNetBase.Core.Contracts.Services.Identity.AccountManagement;
using AspNetBase.Core.Models.Identity.AccountManagement;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetBase.Core.Providers.Services.Identity.AccountManagement
{
  [RegisterDependency(ServiceLifetime.Scoped)]
  public class ManageProfileService : IManageProfileService
  {
    public Task < (UserProfileDto, IEnumerable<string> errors) > GetUserProfile(ClaimsPrincipal loggedInUser)
    {
      return null;
    }

    public Task < (bool saved, IEnumerable<string> errors) > SaveUserProfile(UserProfileDto userProfileDto)
    {
      return null;
    }

    public Task SendVerificationEmail(ClaimsPrincipal loggedInUser, Func<string, int, string> getCallbackUrl)
    {
      return null;
    }
  }
}
