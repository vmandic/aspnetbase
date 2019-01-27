using AspNetBase.Core.App.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace AspNetBase.Core.Contracts.Services.Identity
{
  public interface ISignInService
  {
    Task<(SignInResult, IEnumerable<string> errorMessages)> SignInWithPassword(LoginDto loginDto);

    /// <summary>
    /// Signs out the user for the ASP.NET Identity auth schemes:
    /// <para/>1. Identity.Application
    /// <para/>2. Identity.External
    /// <para/>3. Identity.TwoFactorUserId
    /// </summary>
    Task SignOut();

    Task SignOut(string authenticationScheme);
  }
}
