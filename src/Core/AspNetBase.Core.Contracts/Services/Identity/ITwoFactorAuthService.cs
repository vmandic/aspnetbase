using AspNetBase.Infrastructure.DataAccess.Entities;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace AspNetBase.Core.Contracts.Services.Identity
{
  public interface ITwoFactorAuthService
  {
    Task EnsureUserForTwoFactorAuthentication();
    Task<AppUser> GetTwoFactorAuthenticationUser();
    Task<SignInResult> SignIn(string authCode, bool rememberMe, bool rememberMachine);
    Task<SignInResult> RecoveryCodeSignInAsync(string recoveryCode);
  }
}
