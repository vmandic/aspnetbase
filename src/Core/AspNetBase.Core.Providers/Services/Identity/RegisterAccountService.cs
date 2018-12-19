using AspNetBase.Common.Utils.Attributes;
using AspNetBase.Core.Contracts.Services.Identity;
using AspNetBase.Core.Models.Identity;
using AspNetBase.Infrastructure.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace AspNetBase.Core.Providers.Services.Identity
{
  [RegisterDependency(ServiceLifetime.Scoped)]
  public class RegisterAccountService : IdentityBaseService<RegisterAccountService>, IRegisterAccountService
  {
    private readonly ISignInService _signInService;
    private readonly IEmailSender _emailSender;

    public RegisterAccountService(
      ILogger<RegisterAccountService> logger,
      UserManager<AppUser> userManager,
      SignInManager<AppUser> signInManager,
      ISignInService signInService,
      IEmailSender emailSender) : base(logger, userManager, signInManager)
    {
      this._signInService = signInService;
      this._emailSender = emailSender;
    }

    public async Task<(bool, string callbackUrl, IEnumerable<string> errorMessages)> CreateAccountAndSignIn(
      string email,
      string password,
      Func<string, int, string> getCallbackUrl,
      bool sendEmailConfirmation = true)
    {
      var user = new AppUser { UserName = email, Email = email };
      var result = await userManager.CreateAsync(user, password);

      if (result.Succeeded)
      {
        logger.LogInformation("User created a new account with password.");

        var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
        var callbackUrl = getCallbackUrl(code, user.Id);

        if (sendEmailConfirmation)
          await _emailSender.SendEmailAsync(email, "Confirm your email",
            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

        await _signInService.SignInWithPassword(new LoginDto(email, password, rememberMe: false));

        return (true, callbackUrl, Enumerable.Empty<string>());
      }

      return (false, string.Empty, result.Errors.Select(x => x.Description));
    }

    public async Task<IdentityResult> ConfirmEmailAddress(int userId, string confirmationToken)
    {
      var user = await userManager.Users.SingleOrDefaultAsync(x => x.Id == userId);

      if (user == null)
        return IdentityResult.Failed(
          new IdentityError { Description = $"Unable to load user with ID '{userId}'." });

      var result = await userManager.ConfirmEmailAsync(user, confirmationToken);

      if (!result.Succeeded)
      {
        return IdentityResult.Failed(
          new IdentityError { Description = $"Error confirming email for user with ID '{userId}'." });
      }

      return result;
    }
  }
}
