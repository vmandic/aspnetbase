using System.Threading.Tasks;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;
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