using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AspNetBase.Common.Utils.Attributes;
using AspNetBase.Core.Contracts.Services.Identity.AccountManagement;
using AspNetBase.Core.Models.Identity.AccountManagement;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;
using AspNetBase.Infrastructure.DataAccess.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNetBase.Core.Providers.Services.Identity.AccountManagement
{
  [RegisterDependency(ServiceLifetime.Scoped)]
  public class ManageProfileService : IdentityBaseService<ManageProfileService>, IManageProfileService
  {
    private readonly IUnitOfWork _uow;
    private readonly IEmailSender _emailSender;

    public ManageProfileService(
      ILogger<ManageProfileService> logger,
      UserManager<AppUser> userManager,
      SignInManager<AppUser> signInManager,
      IUnitOfWork uow,
      IEmailSender emailSender) : base(logger, userManager, signInManager)
    {
      _uow = uow;
      _emailSender = emailSender;
    }

    public async Task<UserProfileDto> GetUserProfile(ClaimsPrincipal loggedInUser)
    {
      var user = await GetUserOrThrow(loggedInUser);

      return new UserProfileDto(
        user.UserName,
        user.Email,
        user.PhoneNumber,
        user.EmailConfirmed);
    }

    public async Task SaveUserProfile(ClaimsPrincipal loggedInUser, string email, string phoneNumber)
    {
      var user = await GetUserOrThrow(loggedInUser);

      // TODO: implement validation
      user.Email = email;
      user.PhoneNumber = phoneNumber;

      await _uow.CommitAsync();
      await signInManager.RefreshSignInAsync(user);
    }

    public async Task SendVerificationEmail(ClaimsPrincipal loggedInUser, Func<string, int, string> getCallbackUrl)
    {
      var user = await GetUserOrThrow(loggedInUser);
      var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
      var callbackUrl = getCallbackUrl(code, user.Id);

      await _emailSender.SendEmailAsync(
        user.Email,
        "Confirm your email",
        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
    }
  }
}
