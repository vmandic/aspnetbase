using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetBase.Core.App.Contracts.Services.Identity
{
  public interface IRegisterAccountService
  {
    Task<(bool, string callbackUrl, IEnumerable<string> errorMessages)> CreateAccountAndSignIn(
      string email,
      string password,
      Func<string, int, string> getCallbackUrl,
      bool sendEmailConfirmation = true);

    Task<IdentityResult> ConfirmEmailAddress(int userId, string confirmationToken);
  }
}
