using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetBase.Core.App.Contracts.Services.Admin
{
  public interface IAdminUserService
  {
    Task < (bool, string callbackUrl, IEnumerable<string> errorMessages) > CreateAccount(
      string email,
      string password,
      Func<string, int, string> getCallbackUrl,
      bool sendEmailConfirmation = true);
  }
}
