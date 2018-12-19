using AspNetBase.Common.Utils.Attributes;
using AspNetBase.Core.Contracts.Services.Identity;
using AspNetBase.Infrastructure.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
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
  public class PasswordService : IdentityBaseService<PasswordService>, IPasswordService
  {
    private readonly IEmailSender _emailSender;

    public PasswordService(
      ILogger<PasswordService> logger,
      UserManager<AppUser> userManager,
      SignInManager<AppUser> signInManager,
      IEmailSender emailSender) : base(logger, userManager, signInManager)
    {
      this._emailSender = emailSender;
    }

    public async Task<(bool, string callbackUrl)> ForgotPassword(
      string userEmail,
      Func<string, string> getCallbackUrl,
      bool sendEmailConfirmation = true)
    {
      var user = await userManager.FindByEmailAsync(userEmail);

      if (user == null || !(await userManager.IsEmailConfirmedAsync(user)))
        return (false, string.Empty);

      // For more information on how to enable account confirmation and password reset please 
      // visit https://go.microsoft.com/fwlink/?LinkID=532713
      var code = await userManager.GeneratePasswordResetTokenAsync(user);
      var callbackUrl = getCallbackUrl(code);

      if(sendEmailConfirmation)
        await _emailSender.SendEmailAsync(
            userEmail,
            "Reset Password",
            $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

      return (true, callbackUrl);
    }

    public async Task<(bool, IEnumerable<string> errorMessages)> ResetPassword(string email, string resetToken, string password)
    {
      var user = await userManager.FindByEmailAsync(email);

      if (user == null)
        return (false, Enumerable.Empty<string>());

      var result = await userManager.ResetPasswordAsync(user, resetToken, password);

      if (result.Succeeded)
      {
        logger.LogInformation($"Password reset succesfully for user: '{email}'");
        return (true, Enumerable.Empty<string>());
      }

      return (false, result.Errors.Select(msg => msg.Description));
    }
  }
}
