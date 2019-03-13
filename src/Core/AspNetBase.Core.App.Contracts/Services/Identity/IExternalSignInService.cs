using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AspNetBase.Core.App.Models.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace AspNetBase.Core.App.Contracts.Services.Identity
{
  public interface IExternalSignInService
  {
    ChallengeResult ChallengeExternalLoginProvider(string provider, string redirectUrl);
    Task < (SignInResult, IExternalLoginModel) > SignInWithExternalProvider(string remoteError = null);
    Task < (IdentityResult, IExternalLoginModel, ICollection<string> errorMessages) > ConfirmExternalLogin(string email);
    Task<IEnumerable<AuthenticationScheme>> GetExternalLoginProviders();
  }
}
