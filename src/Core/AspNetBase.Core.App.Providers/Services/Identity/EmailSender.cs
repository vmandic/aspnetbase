using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using AspNetBase.Common.Utils.Attributes;
using AspNetBase.Core.Settings;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNetBase.Core.App.Providers.Services.Identity
{
    [RegisterDependency(ServiceLifetime.Scoped)]
  public class EmailSender : IEmailSender
  {
    private readonly ILogger<EmailSender> _logger;
    private readonly EmailSenderSettings _settings;

    public EmailSender(ILogger<EmailSender> logger, EmailSenderSettings settings)
    {
      this._logger = logger;
      this._settings = settings;
    }

    private SmtpClient GetConfiguredSmtpClient()
    {
      var client = new SmtpClient(_settings.Host, _settings.Port);

      client.Credentials = new NetworkCredential(_settings.Sender, _settings.Password);
      client.EnableSsl = _settings.EnableSsl;

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

      if (_settings.Enabled)
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
        _settings.Sender,
        email,
        subject,
        htmlMessage);

      mail.From = new MailAddress(_settings.From, "[NO-REPLY] aspnetbase.com");
      mail.SubjectEncoding = Encoding.UTF8;
      mail.BodyEncoding = Encoding.UTF8;
      mail.IsBodyHtml = true;

      return mail;
    }
  }
}
