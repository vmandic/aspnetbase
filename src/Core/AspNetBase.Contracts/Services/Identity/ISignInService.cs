using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AspNetBase.Contracts.Services.Identity
{
  public interface ISignInService
  {
    // FEAUTER: external login
    IActionResult ChallengeExternalLoginProvider(string provider, string returnUrl = null);
    Task<IActionResult> LoginWithExternalProvider(string returnUrl = null, string remoteError = null);

    // FEATURE: regular login

    // FEATURE: 2fa login

    // FEATURE: recovery login

    // FEATURE: logout
  }
}
