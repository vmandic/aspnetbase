using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetBase.Core.Contracts.Services.Identity
{
  public interface IPasswordService
  {
    Task<(bool, string callbackUrl)> ForgotPassword(string email, Func<string, string> getCallbackUrl, bool sendEmailConfirmation = true);
    Task<(bool, IEnumerable<string> errorMessages)> ResetPassword(string email, string resetToken, string password);
  }
}
