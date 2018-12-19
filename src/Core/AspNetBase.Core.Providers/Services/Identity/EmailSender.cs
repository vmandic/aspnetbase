using AspNetBase.Common.Utils.Attributes;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AspNetBase.Core.Providers.Services.Identity
{
  [RegisterDependency(ServiceLifetime.Scoped)]
  public class EmailSender : IEmailSender
  {
    private readonly ILogger<EmailSender> _logger;

    public EmailSender(ILogger<EmailSender> logger)
    {
      this._logger = logger;
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
      _logger.LogInformation($"Sending email to: '{email}'");
      // TODO: implement
      return Task.CompletedTask;
    }
  }
}
