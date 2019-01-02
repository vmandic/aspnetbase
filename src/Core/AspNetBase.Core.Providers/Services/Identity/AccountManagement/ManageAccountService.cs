using System;
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

    public async Task<bool> ChangeUserPassword(ClaimsPrincipal loggedInUser, string oldPassword, string newPassword)
    {
      var user = await GetUserOrThrow(loggedInUser);

      var changePasswordResult = await userManager.ChangePasswordAsync(
        user,
        oldPassword,
        newPassword);

      if (changePasswordResult.Succeeded)
      {
        logger.LogInformation("User changed their password successfully.");
        await signInManager.RefreshSignInAsync(user);
      }

      return changePasswordResult.Succeeded;
    }

    public async Task<bool> CheckUserHasPassword(ClaimsPrincipal loggedInUser, AppUser user = null)
    {
      user = user ?? await GetUserOrThrow(loggedInUser);
      return await userManager.HasPasswordAsync(user);
    }

    public async Task<bool> CheckUserPassword(ClaimsPrincipal loggedInUser, string password, AppUser user = null)
    {
      user = user ?? await GetUserOrThrow(loggedInUser);
      return await userManager.CheckPasswordAsync(user, password);
    }

    public async Task<bool> DeleteAccount(ClaimsPrincipal loggedInUser, AppUser user = null)
    {
      user = user ?? await GetUserOrThrow(loggedInUser);
      var result = await userManager.DeleteAsync(user);

      if (!result.Succeeded)
      {
        logger.LogError("User with ID '{UserId}' was not deleted on user's request.", user.Id);
        return false;
      }

      await signInManager.SignOutAsync();

      logger.LogInformation("User with ID '{UserId}' deleted themselves.", user.Id);

      return true;
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
