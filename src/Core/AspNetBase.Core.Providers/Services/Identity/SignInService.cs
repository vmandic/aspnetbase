using AspNetBase.Common.Utils.Attributes;
using AspNetBase.Core.Contracts.Services.Identity;
using AspNetBase.Core.Models.Identity;
using AspNetBase.Infrastructure.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace AspNetBase.Core.Providers.Services.Identity
{
  [RegisterDependency(ServiceLifetime.Scoped)]
  public class SignInService : IdentityBaseService<SignInService>, ISignInService
  {
    public SignInService(
      ILogger<SignInService> logger,
      UserManager<AppUser> userManager,
      SignInManager<AppUser> signInManager,
      IUrlHelper urlHelper) : base(logger, userManager, signInManager, urlHelper) { }


    public Task<(SignInResult, ICollection<string> errorMessages)> SignInWithPassword(LoginDto loginDto)
    {
      throw new System.NotImplementedException();
    }

    public Task SignOut(string authenticationScheme)
    {
      throw new System.NotImplementedException();
    }
  }
}
