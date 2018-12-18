using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AspNetBase.Infrastructure.DataAccess.Entities;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace AspNetBase.Core.Contracts.Services.Identity
{
  public interface ITwoFactorAuthService
  {
    Task EnsureUserForTwoFactorAuthentication();
    Task<AppUser> GetTwoFactorAuthenticationUser();
    Task<SignInResult> SignIn(string authCode, bool rememberMe, bool rememberMachine);
  }
}
