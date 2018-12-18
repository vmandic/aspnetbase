using AspNetBase.Common.Utils.Attributes;
using AspNetBase.Core.Contracts.Services.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace AspNetBase.Core.Providers.Services.Identity
{
  [RegisterDependency(ServiceLifetime.Scoped)]
  public class PasswordService : IPasswordService
  {
    public Task<bool> ResetPassword(string userEmail)
    {
      throw new NotImplementedException();
    }

    public Task<bool> ResetPasswordConfirmation(string userEmail, string resetToken)
    {
      throw new NotImplementedException();
    }
  }
}
