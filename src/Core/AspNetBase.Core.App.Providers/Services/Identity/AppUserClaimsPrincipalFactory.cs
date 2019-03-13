using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetBase.Common.Utils.Attributes;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AspNetBase.Core.App.Providers.Services.Identity
{
  [RegisterDependency(ServiceLifetime.Scoped, typeof(IUserClaimsPrincipalFactory<AppUser>))]
  public class AppUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<AppUser, AppRole>
  {
    public AppUserClaimsPrincipalFactory(
      UserManager<AppUser> userManager,
      RoleManager<AppRole> roleManager,
      IOptions<IdentityOptions> optionsAccessor) : base(userManager, roleManager, optionsAccessor) { }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(AppUser user)
    {
      if (user == null)
        throw new ArgumentNullException(nameof(user));

      var identity = await base.GenerateClaimsAsync(user);
      identity.AddClaim(new Claim("uid", user.Uid.ToString()));

      return identity;
    }
  }
}
