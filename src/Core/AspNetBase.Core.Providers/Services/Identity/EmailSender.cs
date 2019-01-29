using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using AspNetBase.Common.Utils.Attributes;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNetBase.Core.Providers.Services.Identity
{
  [RegisterDependency(ServiceLifetime.Scoped)]
  public class EmailSender : IEmailSender
  {
    private readonly ILogger<EmailSender> _logger;
    private readonly IConfiguration _config;

    public EmailSender(ILogger<EmailSender> logger, IConfiguration config)
    {
      this._logger = logger;
      this._config = config.GetSection("Services:EmailSender");

      if (this._config == null)
        throw new NullReferenceException("Config for section 'Services:EmailSender' is null.");
    }

    private SmtpClient GetConfiguredSmtpClient()
    {
      var client = new SmtpClient(
        _config["Host"],
        int.Parse(_config["Port"]));

      client.Credentials = new NetworkCredential(
        _config["Sender"],
        _config["Password"]);

      client.EnableSsl = bool.Parse(_config["EnableSsl"]);

      return client;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
      if (string.IsNullOrWhiteSpace(email))
        throw new ArgumentException("Argument has an empty string value.", nameof(email));

      if (string.IsNullOrWhiteSpace(subject))
        throw new ArgumentException("Argument has an empty string value.", nameof(subject));

      if (htmlMessage == null)
        throw new ArgumentNullException(nameof(htmlMessage));

      _logger.LogInformation($"Sending email to: '{email}'");

      if (bool.Parse(_config["Enabled"]))
      {
        using(var client = GetConfiguredSmtpClient())
        {
          var mail = ComposeMailMessage(email, subject, htmlMessage);
          await client.SendMailAsync(mail);
        }
      }
      else
      {
        _logger.LogWarning("Email service is not enabled!");
      }
    }

    private MailMessage ComposeMailMessage(string email, string subject, string htmlMessage)
    {
      var mail = new MailMessage(
        _config["Sender"],
        email,
        subject,
        htmlMessage);

      mail.From = new MailAddress(_config["From"], "[NO-REPLY] aspnetbase.com");
      mail.SubjectEncoding = Encoding.UTF8;
      mail.BodyEncoding = Encoding.UTF8;
      mail.IsBodyHtml = true;

      return mail;
    }
  }
}
