using AspNetBase.Core.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace AspNetBase.Core.Contracts.Services.Identity
{
  public interface ISignInService
  {
    Task<(SignInResult, IEnumerable<string> errorMessages)> SignInWithPassword(LoginDto loginDto);
    Task SignOut(string authenticationScheme = null);
  }
}
