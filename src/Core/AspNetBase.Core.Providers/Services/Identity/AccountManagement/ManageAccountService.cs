using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetBase.Common.Utils.Attributes;
using AspNetBase.Core.Contracts.Services.Identity.AccountManagement;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNetBase.Core.Providers.Services.Identity.AccountManagement
{
  [RegisterDependency(ServiceLifetime.Scoped)]
  public class ManageAccountService : IdentityBaseService<ManageAccountService>, IManageAccountService
  {
    public ManageAccountService(
      ILogger<ManageAccountService> logger,
      UserManager<AppUser> userManager,
      SignInManager<AppUser> signInManager) : base(logger, userManager, signInManager) { }

    public async Task<bool> ChangeUserPassword(ClaimsPrincipal loggedInUser, string oldPassword, string newPassword)
    {
      var user = await GetUserOrThrow(loggedInUser);

      // NOTE: handles password strength and rules validation set through ASP.NET Identity
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

      return result.Succeeded;
    }

    public async Task<IDictionary<string, string>> GetPersonalData(ClaimsPrincipal loggedInUser)
    {
      var user = await GetUserOrThrow(loggedInUser);
      logger.LogInformation("User with ID '{UserId}' asked for their personal data.", user.Id);

      // NOTE: Only include personal data for download
      var personalData = new Dictionary<string, string>();

      foreach (var p in GetPersonalDataProps())
        personalData.Add(p.Name, p.GetValue(user)?.ToString() ?? "null");

      return personalData;
    }

    private static IEnumerable<PropertyInfo> GetPersonalDataProps() =>
      typeof(AppUser)
      .GetProperties()
      .Where(p => Attribute.IsDefined(p, typeof(PersonalDataAttribute)));

    public async Task < (bool passwordSet, IEnumerable<string> errors) > SetUserNewPassword(ClaimsPrincipal loggedInUser, string newPassword)
    {
      var user = await GetUserOrThrow(loggedInUser);

      var addPasswordResult = await userManager.AddPasswordAsync(user, newPassword);

      if (!addPasswordResult.Succeeded)
        return (false, addPasswordResult.Errors.Select(x => x.Description));

      await signInManager.RefreshSignInAsync(user);
      return (true, Enumerable.Empty<string>());
    }
  }
}
