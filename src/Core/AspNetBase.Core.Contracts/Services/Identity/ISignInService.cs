using AspNetBase.Core.Models.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace AspNetBase.Core.Contracts.Services.Identity
{
  public interface ISignInService
  {
    // FEATURE: regular login
    Task<(SignInResult, ICollection<string> errorMessages)> SignInWithPassword(LoginDto loginDto);

    // FEATURE: 2fa login

    // FEATURE: recovery login

    // FEATURE: logout
    Task SignOut(string authenticationScheme);
  }
}
