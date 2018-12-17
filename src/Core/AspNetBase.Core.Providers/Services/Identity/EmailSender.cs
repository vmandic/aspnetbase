using AspNetBase.Common.Utils.Attributes;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace AspNetBase.Core.Providers.Services.Identity
{
  [RegisterDependency(ServiceLifetime.Scoped, typeof(IEmailSender))]
  public class EmailSender : IEmailSender
  {
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
      // TODO: implement
      return Task.CompletedTask;
    }
  }
}
