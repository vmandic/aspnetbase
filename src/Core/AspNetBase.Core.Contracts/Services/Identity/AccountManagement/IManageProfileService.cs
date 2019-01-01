using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetBase.Core.Models.Identity.AccountManagement;

namespace AspNetBase.Core.Contracts.Services.Identity.AccountManagement
{
  public interface IManageProfileService
  {
    Task<UserProfileDto> GetUserProfile(ClaimsPrincipal loggedInUser);
    Task SaveUserProfile(ClaimsPrincipal loggedInUser, string email, string phoneNumber);
    Task SendVerificationEmail(ClaimsPrincipal loggedInUser, Func<string, int, string> getCallbackUrl);
  }
}
