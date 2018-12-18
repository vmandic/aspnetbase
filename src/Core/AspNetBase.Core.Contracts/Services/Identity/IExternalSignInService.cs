using System;
using System.Collections.Generic;
using System.Text;

using AspNetBase.Core.Models.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using Microsoft.AspNetCore.Identity;

namespace AspNetBase.Core.Contracts.Services.Identity
{
  public interface IExternalSignInService
  {
    ChallengeResult ChallengeExternalLoginProvider(string provider, string redirectUrl);
    Task<(SignInResult, IExternalLoginModel)> SignInWithExternalProvider(string rootUrl, string returnUrl = null, string remoteError = null);
    Task<(IdentityResult, IExternalLoginModel)> ConfirmExternalLogin(ModelStateDictionary modelState, string email, string returnUrl = null); // TODO: remove modelState, use errorMessages
    Task<IEnumerable<string>> GetExternalLoginProviders();
  }
}
