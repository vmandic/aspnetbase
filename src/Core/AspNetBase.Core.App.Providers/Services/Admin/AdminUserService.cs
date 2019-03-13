using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AspNetBase.Common.Utils.Attributes;
using AspNetBase.Core.App.Contracts.Services.Admin;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNetBase.Core.App.Providers.Services.Admin
{
  [RegisterDependency(ServiceLifetime.Scoped)]
  public class AdminUserService : IAdminUserService
  {
    private readonly UserManager<AppUser> userManager;
    private readonly IEmailSender emailSender;
    private readonly ILogger<AdminUserService> logger;

    public AdminUserService(UserManager<AppUser> userManager, IEmailSender emailSender, ILogger<AdminUserService> logger)
    {
      this.userManager = userManager;
      this.emailSender = emailSender;
      this.logger = logger;
    }

    public async Task < (bool, string callbackUrl, IEnumerable<string> errorMessages) > CreateAccount(
      string email,
      string password,
      Func<string, int, string> getCallbackUrl,
      bool sendEmailConfirmation = true)
    {
      if (string.IsNullOrWhiteSpace(email))
        throw new ArgumentException("Invalid argument value provided.", nameof(email));

      if (string.IsNullOrWhiteSpace(password))
        throw new ArgumentException("Invalid argument value provided.", nameof(password));

      if (getCallbackUrl == null)
        throw new ArgumentNullException(nameof(getCallbackUrl));

      var user = new AppUser { UserName = email, Email = email };
      var result = await userManager.CreateAsync(user, password);

      if (result.Succeeded)
      {
        logger.LogInformation("New user account created by administration.");

        var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
        var callbackUrl = getCallbackUrl(code, user.Id);

        if (sendEmailConfirmation)
          await emailSender.SendEmailAsync(email, "Confirm your email",
            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

        return (true, callbackUrl, Enumerable.Empty<string>());
      }

      return (false, string.Empty, result.Errors.Select(x => x.Description));
    }
  }
}
