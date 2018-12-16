using System.Threading.Tasks;
using AspNetBase.Core.Models.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetBase.Core.Contracts.Services.Identity
{
  public interface ISignInService
  {
    // FEAUTER: external login
    IActionResult ChallengeExternalLoginProvider(string provider, string returnUrl = null);
    Task<(IActionResult, IExternalLoginModel)> LoginWithExternalProvider(string returnUrl = null, string remoteError = null);

    // FEATURE: regular login

    // FEATURE: 2fa login

    // FEATURE: recovery login

    // FEATURE: logout
  }
}
