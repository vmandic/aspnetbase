using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AspNetBase.Common.Utils.Attributes;
using AspNetBase.Common.Utils.Extensions;
using AspNetBase.Core.App.Contracts.Services.Identity.AccountManagement;
using AspNetBase.Core.App.Models.Identity.AccountManagement;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;
using AspNetBase.Infrastructure.DataAccess.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNetBase.Core.App.Providers.Services.Identity.AccountManagement
{
  [RegisterDependency(ServiceLifetime.Scoped)]
  public class ManageProfileService : IdentityBaseService<ManageProfileService>, IManageProfileService
  {
    private readonly IUnitOfWork _uow;
    private readonly IEmailSender _emailSender;
    private readonly IUserValidator<AppUser> _userValidator;

    public ManageProfileService(
      ILogger<ManageProfileService> logger,
      UserManager<AppUser> userManager,
      SignInManager<AppUser> signInManager,
      IUnitOfWork uow,
      IEmailSender emailSender,
      IUserValidator<AppUser> userValidator) : base(logger, userManager, signInManager)
    {
      _uow = uow;
      _emailSender = emailSender;
      _userValidator = userValidator;
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

    public async Task < (bool saved, IEnumerable<string> errors) > SaveUserProfile(ClaimsPrincipal loggedInUser, string email, string phoneNumber)
    {
      var user = await GetUserOrThrow(loggedInUser);

      user.Email = email;
      user.PhoneNumber = phoneNumber;

      var result = await _userValidator.ValidateAsync(userManager, user);

      if (result.Succeeded)
      {
        var saved = await _uow.CommitAsync();

        if (saved)
        {
          await signInManager.RefreshSignInAsync(user);
          logger.LogInformation("User successfully saved changes to profile for user ID '{userId}'", loggedInUser.GetUserId());

          return (saved, Enumerable.Empty<string>());
        }

        logger.LogError("A general error occurred while trying to save a user profile for User ID '{userId}'.", loggedInUser.GetUserId());
        return (false, new List<string> { "A general error occurred while saving to the database." });
      }

      return (false, result.Errors.Select(x => x.Description));
    }

    public async Task SendVerificationEmail(ClaimsPrincipal loggedInUser, Func<string, int, string> getCallbackUrl)
    {
      var user = await GetUserOrThrow(loggedInUser);

      if (getCallbackUrl == null)
        throw new ArgumentNullException(nameof(getCallbackUrl));

      var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
      var callbackUrl = getCallbackUrl(code, user.Id);

      await _emailSender.SendEmailAsync(
        user.Email,
        "Confirm your email",
        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
    }
  }
}
