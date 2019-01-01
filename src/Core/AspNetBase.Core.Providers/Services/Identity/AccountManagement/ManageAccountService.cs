using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetBase.Core.Contracts.Services.Identity.AccountManagement;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AspNetBase.Core.Providers.Services.Identity.AccountManagement
{
  public class ManageAccountService : IdentityBaseService<ManageAccountService>, IManageAccountService
  {
    public ManageAccountService(
      ILogger<ManageAccountService> logger,
      UserManager<AppUser> userManager,
      SignInManager<AppUser> signInManager) : base(logger, userManager, signInManager) { }

    public Task<bool> ChangeUserPassword(ClaimsPrincipal loggedInUser, string oldPassword, string newPassword)
    {
      throw new System.NotImplementedException();
    }

    public Task<bool> CheckUserHasPassword(ClaimsPrincipal loggedInUser)
    {
      throw new System.NotImplementedException();
    }

    public Task<bool> CheckUserPassword(ClaimsPrincipal loggedInUser)
    {
      throw new System.NotImplementedException();
    }

    public Task<bool> DeleteAccount(ClaimsPrincipal loggedInUser)
    {
      throw new System.NotImplementedException();
    }

    public Task<IDictionary<string, string>> GetPersonalData(ClaimsPrincipal loggedInUser)
    {
      throw new System.NotImplementedException();
    }

    public Task<bool> SetUserNewPassword(ClaimsPrincipal loggedInUser, string newPassword)
    {
      throw new System.NotImplementedException();
    }
  }
}
