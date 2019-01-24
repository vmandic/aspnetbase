using System.Security.Principal;
using AspNetBase.Infrastructure.DataAccess.Enums;

namespace AspNetBase.Presentation.Server.Extensions
{
  public static class IPrincipalExtensions
  {
    public static bool IsInRole(this IPrincipal principal, Roles role) =>
      principal.IsInRole(role.ToString());
  }
}
